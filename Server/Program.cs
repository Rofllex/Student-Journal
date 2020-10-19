using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitializeLogging();
            InitializeDatabase();
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void InitializeDatabase()
        {
            Database.JournalDbContext.SetConnectionString("Server=localhost; Database=studentjournal; Uid=root; Pwd=root");
            using (Database.JournalDbContext dbContext = new Database.JournalDbContext())
                dbContext.Database.EnsureCreated();
        }

        private static void InitializeLogging() => Logging.Logger.SetInstance(new Logging.ConsoleLogger());
    }
}
