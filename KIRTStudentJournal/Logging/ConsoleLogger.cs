using System;

namespace KIRTStudentJournal.Logging
{
    public class ConsoleLogger : Logger
    {
        public override void Cause(string message) => WriteLineSynced("Cause", ConsoleColor.Blue, message);
        public override void Cause(Exception e) => Cause(e.ToString());
        public override void Error(string message) => WriteLineSynced("Error", ConsoleColor.Red, message);
        public override void Error(Exception e) => Error(e.ToString());
        public override void Fatal(string message) => WriteLineSynced("Fatal", ConsoleColor.DarkRed, message);
        public override void Fatal(Exception e) => Fatal(e.ToString());
        public override void Info(string message) => WriteLineSynced("Info", ConsoleColor.Green, message);
        public override void Warning(string message) => WriteLineSynced("Warning", ConsoleColor.Yellow, message);
        public override void Warning(Exception e) => Warning(e.ToString());

        private object _mutex = new object();
        protected virtual void WriteLineSynced(string prefix, ConsoleColor prefixColor, string message)
        {
            lock (_mutex)
            {
                Console.Write(GetTimeStamp() + " [");
                var backForeColor = Console.ForegroundColor;
                Console.ForegroundColor = prefixColor;
                Console.Write(prefix);
                Console.ForegroundColor = backForeColor;
                Console.WriteLine("] " + message);
            }
        }

        protected string GetTimeStamp()
        {
            return DateTime.Now.ToString();
        }
    }
}
