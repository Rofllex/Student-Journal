using System;
using System.IO;

namespace KIRTStudentJournal.Logging
{
    public class StreamWriterLogger : ConsoleLogger
    {
        private StreamWriter _sw;
        public StreamWriterLogger(StreamWriter sw)
        {
            _sw = sw;
        }

        protected override void WriteLineSynced(string prefix, ConsoleColor prefixColor, string message)
        {
            _sw.WriteLine($"{GetTimeStamp()} [{prefix}] {message}");
            _sw.Flush();
        }
    }
}
