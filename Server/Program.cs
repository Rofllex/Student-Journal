using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Server.Database;

namespace Server
{
    public class Program
    {
        public static JToken Config { get; private set; }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitializeLogging();
            LoadConfig();
            InitializeDatabase();
            
            CreateHostBuilder(args).Build().Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logging.Logger.Instance.Fatal("Необработанное исключение");
            Logging.Logger.Instance.Fatal((Exception)e.ExceptionObject);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void InitializeDatabase()
        {
            string connectionString = Config["db"]["connection_string"].ToObject<string>();
            JournalDbContext.SetConnectionString(connectionString);
            using (JournalDbContext dbContext = new Database.JournalDbContext())
            {
                IEnumerable<string> rolesToCreate;
                if (dbContext.Database.EnsureCreated())
                    rolesToCreate = Role.DefaultRoles;
                else
                    rolesToCreate = Role.DefaultRoles.Except(dbContext.Roles.ToList().ConvertAll(u => u.Name));
                using (IEnumerator<string> enumerator = rolesToCreate.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        do
                        {
                            string roleName = enumerator.Current;
                            Role role = new Role(roleName);
                            dbContext.Roles.Add(role);
                        } while (enumerator.MoveNext());
                        dbContext.SaveChanges();
                    }
                }
            }
            Logging.Logger.Instance.Info("База данных инициализирована");
        }

        private static void LoadConfig()
        {
            string jsonString;
            using (StreamReader reader = File.OpenText("Config/config.json"))
                jsonString = reader.ReadToEnd();
            Config = JsonConvert.DeserializeObject<JToken>(jsonString);
        }

        private static void InitializeLogging() => Logging.Logger.SetInstance(new Logging.ConsoleLogger());
    }
}
