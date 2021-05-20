using System;
using System.IO;

namespace Journal.Logging
{
    /// <summary>
    ///     Базовый класс логгера.
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        ///     Отладочный лог.
        /// </summary>
        /// <param name="message">
        ///     Сообщение.
        /// </param>
        public abstract void Debug( string message );

        /// <summary>
        ///     Информационный лог.
        /// </summary>
        /// <param name="message">
        ///     Сообщение
        /// </param>
        public abstract void Info( string message );
        
        /// <summary>
        ///     Лог предупреждения.
        /// </summary>
        /// <param name="message">
        ///     Сообщение
        /// </param>
        public abstract void Warning( string message );

        /// <summary>
        ///     Лог предупреждения при исключении.
        /// </summary>
        /// <param name="ex">
        ///     Исключение.
        /// </param>
        public abstract void Warning( Exception ex );

        /// <summary>
        ///     Лог ошибки.
        /// </summary>
        /// <param name="message">
        ///     Сообщение.
        /// </param>
        public abstract void Error( string message );

        /// <summary>
        ///     Лог ошибки.
        /// </summary>
        /// <param name="ex">
        ///     Исключение.
        /// </param>
        public abstract void Error( Exception ex );

        /// <summary>
        ///     Лог фатальной ошибки.
        /// </summary>
        /// <param name="message">
        ///     Сообщение.
        /// </param>
        public abstract void Fatal( string message );
        
        /// <summary>
        ///     Лог фатальной ошибки
        /// </summary>
        /// <param name="ex">
        ///     Исключение.
        /// </param>
        public abstract void Fatal( Exception ex );

        /// <summary>
        ///     Лог причины.
        /// </summary>
        /// <param name="message">
        ///     Сообщение.
        /// </param>
        public abstract void Cause( string message );
        
        /// <summary>
        ///     Лог причины.
        /// </summary>
        /// <param name="ex">
        ///     Исключение.
        /// </param>
        public abstract void Cause( Exception ex );

        public static Logger Instance { get; set; }
    }

    /// <summary>
    ///     Комбинированный логгер.
    ///     Позволяет использовать несколько логгеров одновременно.
    /// </summary>
    public sealed class CombinedLogger : Logger
    {
        public CombinedLogger(params Logger[] loggers)
        {
            _loggers = loggers ?? throw new ArgumentNullException(nameof(loggers));
            if ( _loggers.Length == 0 )
                throw new ArgumentException($"{nameof(_loggers)} не может быть 0!");
        }

        public override void Debug( string message )
            => _InvokeLogger( l => l.Debug( message ) );

        public override void Info( string message )
            => _InvokeLogger( l => l.Info( message ) );

        public override void Warning( string message )
         => _InvokeLogger( l => l.Info( message ) );

        public override void Warning( Exception ex )
         => _InvokeLogger( l => l.Warning( ex ) );

        public override void Error( string message )
         => _InvokeLogger( l => l.Error( message ) );

        public override void Error( Exception ex )
        => _InvokeLogger( l => l.Error( ex ) );

        public override void Fatal( string message )
         => _InvokeLogger( l => l.Fatal( message ) );

        public override void Fatal( Exception ex )
         => _InvokeLogger( l => l.Fatal( ex ) );

        public override void Cause( string message )
        => _InvokeLogger( l => l.Cause( message ) );

        public override void Cause( Exception ex )
         => _InvokeLogger( l => l.Cause( ex ) );

        private Logger[] _loggers;
        private void _InvokeLogger( Action<Logger> act )
        {
            for ( int i = 0; i < _loggers.Length; i++ )
                act( _loggers[i] );
        }
    }

    /// <summary>
    ///     Логгер в консоли <see cref="System.Console"/>
    /// </summary>
    public class ConsoleLogger : Logger
    {
        public override void Cause( string message )
            => WriteLine( nameof(Cause), message, ConsoleColor.Blue );

        public override void Cause( Exception ex )
            => Cause( ex.ToString() );

        public override void Debug( string message )
            => WriteLine( nameof(Debug), message, ConsoleColor.Cyan );

        public override void Error( string message )
            => WriteLine( nameof(Error), message, ConsoleColor.Red );

        public override void Error( Exception ex )
            => Error( ex.ToString() );

        public override void Fatal( string message )
            => WriteLine( nameof(Fatal), message, ConsoleColor.DarkRed );

        public override void Fatal( Exception ex )
            => Fatal( ex.ToString() );

        public override void Info( string message ) 
            => WriteLine( nameof(Info), message, ConsoleColor.Green );

        public override void Warning( string message ) 
            => WriteLine( nameof(Warning), message, ConsoleColor.Yellow );

        public override void Warning( Exception ex )
            => Warning( ex.ToString() );


        protected virtual void WriteLine( string prefix, string text, ConsoleColor foregroundColor )
        {
            _LockInvoke( () =>
            {
                Console.Write(DateTime.Now + " ");

                ConsoleColor tempForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
                Console.Write($"[{prefix}]");
                Console.ForegroundColor = tempForegroundColor;
                Console.WriteLine($" {text}");
            } );
        }
        
        private void _LockInvoke( Action act )
        {
            lock ( _consoleMutex )
            {
                act();
            }
        }


        private static object _consoleMutex = new object();
    }

    /// <summary>
    ///     Логгер записей в <see cref="StreamWriter"/>
    ///     <para>Наследуется от <see cref="ConsoleLogger"/></para>
    /// </summary>
    /// <remarks>
    ///     Данный класс не наследуется.
    /// </remarks>
    public sealed class StreamWriterLogger : ConsoleLogger
    {
        public StreamWriterLogger(StreamWriter sw) 
        {
            _sw = sw ?? throw new ArgumentNullException();
        }

        protected override void WriteLine( string prefix, string text, ConsoleColor _ )
        {
            lock ( _mutex )
            {
                _sw.WriteLine( $"{DateTime.Now} [{prefix}] {text}" );
                _sw.Flush();
            }
        }

        
        private object _mutex = new object();
        private StreamWriter _sw;

        #region public static

        /// <summary>
        /// Создать логгер из файла.
        /// </summary>
        /// <param name="filePath">
        ///     Полный путь до файла.
        /// </param>
        /// <param name="deleteExisted">
        ///     Если файл существует и его необходимо перезаписать.
        /// </param>
        /// <returns>
        ///     Возвращает <c>default</c> если файл присутствует и <paramref name="deleteExisted"/> <c>== false</c>
        /// </returns>
        public static StreamWriterLogger CreateTextFile( string filePath, bool deleteExisted = true )
        {
            if ( File.Exists( filePath ) )
            {
                if ( deleteExisted )
                    File.Delete( filePath );
                else
                    return default;
            }

            StreamWriter sw = File.CreateText( filePath );
            return new StreamWriterLogger( sw );
        }

        #endregion
    }
}
