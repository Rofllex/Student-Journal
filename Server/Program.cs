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
                logger.Info("������������ ���������");

                _InitializeDatabase();
                logger.Info("���� ������ ����������������");

            }
            catch (Exception e)
            {
                logger.Fatal("������ ��� ������������� �������.");
                logger.Cause(e);
                return;
            }

#if DEBUG
            // ���� � ������������ ��������� ��� ���������� ��������� �� �����.
            if (Server.Config.JournalConfiguration.Single.Pause 
                // ��� ���� ��� ������� � ����������
                || args.FirstOrDefault(a => a == "+pause" && a == "+p") != default)
            {
                Console.WriteLine("������� ����� ������ ����� ����������...");
                Console.ReadKey(true);
                Console.WriteLine("����������� ���������� ����������.");
            }
#endif
            CreateHostBuilder(args).Build().Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Logging.Logger.Instance != null)
            {
                Logging.Logger.Instance.Fatal("�������������� ����������");
                Logging.Logger.Instance.Fatal((Exception)e.ExceptionObject);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} [FATAL NO LOGGER] �������������� ����������!\n{e.ExceptionObject.ToString()}");
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
        /// ����� ������������� ���� ������.
        /// ���� ���� ������ ���� ������ ��� �������, �� ��������� ������� ������ root �� ���������.
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
        /// �������� ������������
        /// </summary>
        private static void _LoadConfig()
            => Server.Config.JournalConfiguration.FromFile(Path.Combine(ExecutableDirectory, "Config", "config.json"));
        

        /// <summary>
        /// ������������� �������.
        /// </summary>
        private static void _InitializeLogging() => Logger.SetInstance(new ConsoleLogger());
    }
}
