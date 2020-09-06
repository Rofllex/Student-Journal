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

namespace KIRTStudentJournal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.Instance = new CombinedLogger(new ConsoleLogger(), new StreamWriterLogger(FileLogging.CreateLogFile(out string _)));
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
