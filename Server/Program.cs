using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Journal.Server.Database;
using Journal.Server.Logging;

namespace Journal.Server
{
    public class Program
    {
        public static JToken Config { get; private set; }

        public static readonly string ExecutableDirectory = ExecutableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static Program() 
        {
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _InitializeLogging();
            ILogger logger = Logger.Instance;
            try
            {
                _LoadConfig();
                logger.Info("Конфигурация загружена");

                _InitializeDatabase();
                logger.Info("База данных инициализирована");

            }
            catch (Exception e)
            {
                logger.Fatal("Ошибка при инициализации сервера.");
                logger.Cause(e);
                return;
            }

#if DEBUG
            // Если в конфигурации прописано что необходимо поставить на паузу.
            if (Server.Config.JournalConfiguration.Single.Pause 
                // Или если это указано в аргументах
                || args.FirstOrDefault(a => a == "+pause" && a == "+p") != default)
            {
                Console.WriteLine("Нажмите любую кнопку чтобы продолжить...");
                Console.ReadKey(true);
                Console.WriteLine("Продолжение выполнения приложения.");
            }
#endif
            CreateHostBuilder(args).Build().Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Logging.Logger.Instance != null)
            {
                Logging.Logger.Instance.Fatal("Необработанное исключение");
                Logging.Logger.Instance.Fatal((Exception)e.ExceptionObject);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} [FATAL NO LOGGER] Необработанное исключение!\n{e.ExceptionObject.ToString()}");
                Console.WriteLine(e.ExceptionObject.ToString());
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        /// <summary>
        /// Метод инициализации базы данных.
        /// Если база данных была только что создана, то создается учетная записо root по умолчанию.
        /// </summary>
        private static void _InitializeDatabase()
        {
            string connectionString = Server.Config.JournalConfiguration.Single.Database.ConnectionString;
            JournalDbContext.SetConnectionString(connectionString);
            using (JournalDbContext dbContext = JournalDbContext.CreateContext())
            {
                if (dbContext.Database.EnsureCreated())
                {
                    dbContext.Users.Add(new User()
                    {
                        Login = "root",
                        PasswordHash = Security.Hash.GetFromString("root"),
                        FirstName = "root",
                        LastName = "root",
                        Surname = "root",
                        //Role = Common.Entities.UserRole.Admin,
                    });
                    dbContext.SaveChanges();
                }
            }
        }
                                                                           
        /// <summary>
        /// Загрузка конфигурации
        /// </summary>
        private static void _LoadConfig()
            => Server.Config.JournalConfiguration.FromFile(Path.Combine(ExecutableDirectory, "Config", "config.json"));
        

        /// <summary>
        /// Инициализация логгера.
        /// </summary>
        private static void _InitializeLogging() => Logger.SetInstance(new ConsoleLogger());
    }
}
