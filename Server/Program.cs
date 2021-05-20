using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Journal.Server.Database;
using Journal.Logging;

namespace Journal.Server
{
    public class Program
    {
        /// <summary>
        ///     ���������� �������.
        /// </summary>
        public static readonly string ExecutableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _InitializeLogging();
            Logger logger = Logger.Instance;
            try
            {
                _LoadConfig();
                logger.Info($"{nameof(Main)} ������������ ���������");

                _InitializeDatabase();
                logger.Info($"{ nameof(Main) } ���� ������ ����������������");

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
                || args.FirstOrDefault(a => a == "--pause" || a == "-p") != default)
            {
                Console.WriteLine("������� ����� ������ ����� ����������...");
                Console.ReadKey(true);
                Console.WriteLine("����������� ���������� ����������.");
            }
#endif
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///     ���������� ���������������� ����������.
        /// </summary>
        /// <param name="sender">
        ///     ������, ��������� ����������.
        /// </param>
        /// <param name="e">
        ///     �������� �������
        /// </param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Logger.Instance != null)
            {
                Logger.Instance.Fatal($"{nameof(CurrentDomain_UnhandledException)} �������������� ����������");
                Logger.Instance.Fatal((Exception)e.ExceptionObject);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} [FATAL NO LOGGER] �������������� ����������!\n{e.ExceptionObject}");
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
        ///     ����� ������������� ���� ������.
        ///     ���� ���� ������ ���� ������ ��� �������, �� ��������� ������� ������ root �� ���������.
        /// </summary>
        private static void _InitializeDatabase()
        {
            string connectionString = Server.Config.JournalConfiguration.Single.Database.ConnectionString;
            JournalDbContext.SetConnectionString(connectionString);
            using (JournalDbContext dbContext = JournalDbContext.CreateContext())
            {
                if (dbContext.Database.EnsureCreated())
                {
                    User user = new User(firstName: "root"
                        , surname: "root"
                        , login: "root"
                        , passwordHash: Security.Hash.GetFromString("root")
                        , role: Common.Entities.UserRole.Admin);

                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    Logger.Instance.Info($"{nameof(_InitializeDatabase)} ������� ���� ������ � ������������� �� ���������.");
                }
            }
        }
                                                                           
        /// <summary>
        ///     �������� ������������
        /// </summary>
        private static void _LoadConfig()
            => Server.Config.JournalConfiguration.FromFile(Path.Combine(ExecutableDirectory, "Config", "config.json"));
        

        /// <summary>
        ///     ������������� �������.
        /// </summary>
        private static void _InitializeLogging() => Logger.Instance = new ConsoleLogger();
    }
}
