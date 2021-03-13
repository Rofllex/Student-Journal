using System;

using Journal.Logging;

#nullable enable

namespace Journal.WindowsForms
{
    public delegate void LogEventHandler(EventLogType logType, string? message, Exception? ex);
    
    /// <summary>
    ///     Тип лога.
    /// </summary>
    public enum EventLogType
    {
        /// <summary>
        ///     Отладочный лог
        /// </summary>
        Debug,
        /// <summary>
        ///     Информационный лог
        /// </summary>
        Info,
        /// <summary>
        ///     Лог предупреждения
        /// </summary>
        Warning,
        /// <summary>
        ///     Лог ошибки
        /// </summary>
        Error,
        /// <summary>
        ///     Лог фатальной ошибки
        /// </summary>
        Fatal,
        /// <summary>
        ///     Лог причины.
        /// </summary>
        Cause
    }

    public class EventLogger : Logger
    {
        /// <summary>
        ///     Пустое событие по умолчанию. NullReferenceException pattern.
        /// </summary>
        public event LogEventHandler Log = (_,__,___) => { };

        public override void Cause( string message )
            => OnLog( EventLogType.Cause, message, null );

        public override void Cause( Exception ex )
            => OnLog( EventLogType.Cause, null, ex );

        public override void Debug( string message )
            => OnLog( EventLogType.Debug, message, null );

        public override void Error( string message )
            => OnLog( EventLogType.Debug, message, null );

        public override void Error( Exception ex )
            => OnLog( EventLogType.Error, null, ex );

        public override void Fatal( string message )
            => OnLog( EventLogType.Fatal, message, null );

        public override void Fatal( Exception ex )
            => OnLog( EventLogType.Fatal, null, ex );

        public override void Info( string message )
            => OnLog( EventLogType.Info, message, null );

        public override void Warning( string message )
            => OnLog( EventLogType.Warning, message, null );

        public override void Warning( Exception ex )
            => OnLog( EventLogType.Warning, null, ex );


        protected void OnLog( EventLogType logType, string? message, Exception? ex )
            => Log( logType, message, ex );
    }
}
