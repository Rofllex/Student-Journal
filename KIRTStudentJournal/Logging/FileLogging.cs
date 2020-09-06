using System;
using System.Reflection;
using System.IO;

namespace KIRTStudentJournal.Logging
{
    public static class FileLogging
    {
        public const string LOGS_DIRECTORY_NAME = "logs";
        private const string _LOG_FILE_NAME_FORMAT = "dd.MM.yyyy-HH.mm.ss";
        public static StreamWriter CreateLogFile()
        {
            var asm = Assembly.GetExecutingAssembly();
            var executingAssemblyDirectoryPath = Path.GetDirectoryName(asm.Location);
            var filePath = Path.Combine(executingAssemblyDirectoryPath, LOGS_DIRECTORY_NAME, $"log-{DateTime.Now.ToString(_LOG_FILE_NAME_FORMAT)}.txt");
            return File.CreateText(filePath);
        }

        public static StreamWriterLogger CreateLogger() => new StreamWriterLogger(CreateLogFile());
        
    }
}
