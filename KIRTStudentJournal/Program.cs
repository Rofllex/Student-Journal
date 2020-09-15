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
using KIRTStudentJournal.Database.Journal;
using System.Runtime.CompilerServices;

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
                        config = JsonConvert.DeserializeObject<JToken>(reader.ReadToEnd());
                    string dbConnectionString = config["database"]["connectionString"].ToObject<string>();
                    Database.DatabaseContext.ConnectionString = dbConnectionString;
                    using (Database.DatabaseContext db = new Database.DatabaseContext())
                    {
                        if (db.Database.EnsureCreated())
                            createDefaultEntities(db);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Logger.Instance.Fatal("�� ������� ��������� ���� ������������");
                    Console.ReadKey(true);
                    return;
                }
                catch (Exception e)
                {
                    Logger.Instance.Fatal(e);
                    Console.ReadKey(true);
                    return;
                }
            }
            else
            {
                Logger.Instance.Fatal("�� ������ ���� ������������");
                Console.ReadKey(true);
                return;
            }
            CreateHostBuilder(args).Build().Run();
        }
        
        private static void createDefaultEntities(Database.DatabaseContext dbContext) 
        {
            var specialization = new Specialization()
            {
                Name = "testSpecialization",
                ShortName = "ts"
            };
            dbContext.Specializations.Add(specialization);

            var group = new StudentGroup(1, 1, specialization);
            dbContext.StudentGroups.Add(group);

            var account = new Database.Account()
            {
                Login = "12345",
                PasswordHash = Infrastructure.Hash.GetHashFromString("1"), // sha256(1)
                Person = new Student()
                {
                    FirstName = "1",
                    LastName = "1",
                    Patronymic = "1",
                    PhoneNumber = "1",
                },
                Role = Role.Admin
            };
            account.Person.Account = account;
            dbContext.Accounts.Add(account);

            group.Students = new List<Person>();
            group.Students.Add(account.Person);

            dbContext.Subjects.Add(new Subject()
            {
                Name = "testSubject",
                ShortName = "testSubj"
            });
            
            

            dbContext.SaveChanges();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Instance.Fatal("�������������� ����������");
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
