using System;
using System.Collections.Generic;
using System.Text;
using Journal.Logging;

#nullable enable

namespace Journal.WindowsForms
{
    public class EventLogger : Logger
    {
        public enum EventLogType
        {
            Debug,
            Info,
            Warning,
            Error,
            Fatal,
            Cause
        }

        public EventLogger()
        {

        }

        public event LogEventHandler Log = (_,__,___) => { };
        public delegate void LogEventHandler( EventLogType logType, string? message, Exception? ex );



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
