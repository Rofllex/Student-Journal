using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using KIRTStudentJournal.Logging;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Diagnostics;

namespace KIRTStudentJournal
{
    public class Program
    {
        public static readonly string ExecutableRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Logger.Instance = new CombinedLogger(new ConsoleLogger(), new StreamWriterLogger(FileLogging.CreateLogFile(out string _)));
            string configPath = Path.Combine(ExecutableRootPath, "Config", "cfg.json");
            if (File.Exists(configPath))
            {
                try
                {
                    JToken config;
                    using (var reader = File.OpenText(configPath))
                    {
                        config = JsonConvert.DeserializeObject<JToken>(reader.ReadToEnd());
                    }
                    string dbConnectionString = config["database"]["connectionString"].ToObject<string>();
                    Database.DatabaseContext.ConnectionString = dbConnectionString;
                    //using (var db = new Database.DatabaseContext())
                    //{
                    //    db.Accounts.Add(new Database.Account()
                    //    {
                    //        Login = "12345",
                    //        PasswordHash = "c4ca4238a0b923820dcc509a6f75849b", // md5(1)
                    //        Person = new Database.Person()
                    //        {
                    //            FirstName = "1",
                    //            LastName = "1",
                    //            Patronymic = "1",
                    //            PhoneNumber = "1",
                    //        },
                    //        Role = Database.Role.Admin
                    //    });
                    //    db.SaveChanges();
                    //}
                }
                catch (UnauthorizedAccessException)
                {
                    Logger.Instance.Fatal("Не удалось прочитать файл конфигурации");
                    Console.ReadKey(true);
                    return;
                }
            }
            else
            {
                Logger.Instance.Fatal("Не найден файл конфигурации");
                Console.ReadKey(true);
                return;
            }
            CreateHostBuilder(args).Build().Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Instance.Fatal("Необработанное исключение");
            Logger.Instance.Fatal((Exception)e.ExceptionObject);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
