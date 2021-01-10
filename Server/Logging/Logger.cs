using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Journal.Server.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Отладочный лог. Можно оставить пустым, если сборка Release.
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Warning(Exception e);
        void Error(string message);
        void Error(Exception e);
        void Fatal(string message);
        void Fatal(Exception e);
        void Cause(string message);
        void Cause(Exception e);
    }

    public abstract class Logger : ILogger
    {
        public abstract void Debug(string message);
        public abstract void Info(string message);
        public abstract void Warning(string message);
        public abstract void Warning(Exception e);
        public abstract void Error(string message);
        public abstract void Error(Exception e);
        public abstract void Fatal(string message);
        public abstract void Fatal(Exception e);
        public abstract void Cause(string message);
        public abstract void Cause(Exception e);

        public static ILogger Instance { get; private set; }
        public static void SetInstance(Logger instance)
        {
            if (Instance == null)
                Instance = instance;
        }

    }

    /// <summary>
    /// Класс записи логов в консоль.
    /// </summary>
    public class ConsoleLogger : Logger
    {
        public override void Debug(string message) => Log("Debug", message, ConsoleColor.Cyan);

        public override void Cause(Exception e) => Cause(e.ToString());

        public override void Cause(string message) => Log("Cause", message, ConsoleColor.Cyan);

        public override void Error(string message) => Log("Error", message, ConsoleColor.Red);

        public override void Error(Exception e) => Error(e.ToString());

        public override void Fatal(string message) => Log("Fatal", message, ConsoleColor.DarkRed);

        public override void Fatal(Exception e) => Fatal(e.ToString());

        public override void Info(string message) => Log("Info", message, ConsoleColor.Green);

        public override void Warning(string message) => Log("Warning", message, ConsoleColor.Yellow);

        public override void Warning(Exception e) => Warning(e.ToString());

        /// <summary>
        /// Семафор синхронизации доступа к классу <see cref="Console"/>
        /// </summary>
        protected static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

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

    /// <summary>
    /// Логгер записывающий данные в <see cref="StreamWriter"/>.
    /// Можно использовать для записи в файл.
    /// Наследуется от <see cref="ConsoleLogger"/>
    /// </summary>
    public class StreamWriterLogger : ConsoleLogger
    {
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

        private readonly StreamWriter _sw;
    }

    /// <summary>
    /// Комбинированный логгер.
    /// Прослойка для возможности использования нескольких логгеров.
    /// Наследуется от <see cref="Logger"/>
    /// </summary>
    public class CombinedLogger : ILogger
    {
        /// <summary>
        /// Конструктор класса комбинированного логгера.
        /// </summary>
        /// <param name="loggers"></param>
        /// <exception cref="ArgumentException">Если кол-вол логеров равно.</exception>
        public CombinedLogger(params ILogger[] loggers)
        {
            if (loggers.Length > 0)
                _loggers = loggers;
            throw new ArgumentException("Кол-во логгеров не может быть 0!");
        }

        public CombinedLogger(IEnumerable<ILogger> loggersEnumerable) : this (loggersEnumerable.ToArray())
        {
        }

        public void Debug(string message)       => _OnLog(l => l.Debug(message));
        public void Cause(Exception e)          => _OnLog(l => l.Cause(e));
        public void Cause(string message)       => _OnLog(l => l.Cause(message));
        public void Error(string message)       => _OnLog(l => l.Error(message));
        public void Error(Exception e)          => _OnLog(l => l.Error(e));
        public void Fatal(string message)       => _OnLog(l => l.Fatal(message));
        public void Fatal(Exception e)          => _OnLog(l => l.Fatal(e));
        public void Info(string message)        => _OnLog(l => l.Info(message));
        public void Warning(string message)     => _OnLog(l => l.Warning(message));
        public void Warning(Exception e)        => _OnLog(l => l.Warning(e));

        
        private readonly ILogger[] _loggers;
        private void _OnLog(Action<ILogger> act)
        {
            for (int i = 0; i < _loggers.Length; i++)
                act(_loggers[i]);
        }
    }
}
