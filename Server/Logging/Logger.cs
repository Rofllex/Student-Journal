using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Journal.Server.Logging
{
    public abstract class Logger
    {
        public abstract void Info(string message);
        public abstract void Warning(string message);
        public abstract void Warning(Exception e);
        public abstract void Error(string message);
        public abstract void Error(Exception e);
        public abstract void Fatal(string message);
        public abstract void Fatal(Exception e);
        public abstract void Cause(string message);
        public abstract void Cause(Exception e);

        public static Logger Instance { get; private set; }
        public static void SetInstance(Logger instance)
        {
            if (Instance == null)
                Instance = instance;
        }
    }


    public class ConsoleLogger : Logger
    {
        protected readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

        public override void Cause(Exception e) => Cause(e.ToString());

        public override void Cause(string message) => Log("Cause", message, ConsoleColor.Cyan);

        public override void Error(string message) => Log("Error", message, ConsoleColor.Red);

        public override void Error(Exception e) => Error(e.ToString());

        public override void Fatal(string message) => Log("Fatal", message, ConsoleColor.DarkRed);

        public override void Fatal(Exception e) => Fatal(e.ToString());

        public override void Info(string message) => Log("Info", message, ConsoleColor.Green);

        public override void Warning(string message) => Log("Warning", message, ConsoleColor.Yellow);

        public override void Warning(Exception e) => Warning(e.ToString());

        protected virtual void Log(string prefix, string text, ConsoleColor prefixTextColor)
        {
            Semaphore.Wait();

            Console.Write(DateTime.Now.ToString("G") + " ");
            var predTextColor = Console.ForegroundColor;
            Console.ForegroundColor = prefixTextColor;
            Console.Write($"[{prefix}]");
            Console.ForegroundColor = predTextColor;
            Console.WriteLine(" " + text);

            Semaphore.Release();
        }

    }

    public class StreamWriterLogger : ConsoleLogger
    {
        private readonly StreamWriter _sw;

        public StreamWriterLogger(StreamWriter sw)
        {
            _sw = sw ?? throw new ArgumentNullException(nameof(sw));
        }

        protected override void Log(string prefix, string text, ConsoleColor _)
        {
            Semaphore.Wait();

            _sw.WriteLine($"{DateTime.Now:G} [{prefix}] {text}");

            Semaphore.Release();
        }
    }

    public class CombinedLogger : Logger
    {
        private readonly Logger[] _loggers;

        public CombinedLogger(params Logger[] loggers)
        {
            _loggers = loggers;
        }

        public CombinedLogger(IEnumerable<Logger> loggers) : this (loggers.ToArray())
        {
        }
        
        public override void Cause(Exception e) => OnLog(l => l.Cause(e));
        public override void Cause(string message) => OnLog(l => l.Cause(message));
        public override void Error(string message) => OnLog(l => l.Error(message));
        public override void Error(Exception e) => OnLog(l => l.Error(e));
        public override void Fatal(string message) => OnLog(l => l.Fatal(message));
        public override void Fatal(Exception e) => OnLog(l => l.Fatal(e));
        public override void Info(string message) => OnLog(l => l.Info(message));
        public override void Warning(string message) => OnLog(l => l.Warning(message));
        public override void Warning(Exception e) => OnLog(l => l.Warning(e));

        private void OnLog(Action<Logger> act)
        {
            for (int i = 0; i < _loggers.Length; i++)
                act(_loggers[i]);
        }
    }
}
