using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using KIRTStudentJournal.Logging;

namespace KIRTStudentJournal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.Instance = new CombinedLogger(new ConsoleLogger(), new StreamWriterLogger(FileLogging.CreateLogFile()));
            var logger = Logger.Instance;
            logger.Info("info");
            logger.Warning("warning");
            logger.Cause("cause");
            logger.Error("error");
            logger.Fatal("fatal");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
