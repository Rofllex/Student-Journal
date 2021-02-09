using System;

namespace StudentJournal.Logger
{
    public abstract class Logger
    {
        public abstract void Debug( string message );
        public abstract void Info( string message );
        public abstract void Warning( string message );
        public abstract void Warning( Exception ex );
        public abstract void Error( string message );
        public abstract void Error( Exception ex );
        public abstract void Fatal( string message );
        public abstract void Fatal( Exception ex );
        public abstract void Cause( string message );
        public abstract void Cause( Exception ex );

        public static Logger Instance { get; set; }
    }

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

    public class ConsoleLogger : Logger
    {
        public override void Cause( string message )
            => _WriteLineColorized( message, ConsoleColor.Blue );

        public override void Cause( Exception ex )
            => Cause( ex.ToString() );

        public override void Debug( string message )
            => _WriteLineColorized( message, ConsoleColor.Cyan );

        public override void Error( string message )
            => _WriteLineColorized( message, ConsoleColor.Red );

        public override void Error( Exception ex )
            => Error( ex.ToString() );

        public override void Fatal( string message )
            => _WriteLineColorized( message, ConsoleColor.DarkRed );

        public override void Fatal( Exception ex )
            => Fatal( ex.ToString() );

        public override void Info( string message ) 
            => _WriteLineColorized( message, ConsoleColor.Green );

        public override void Warning( string message ) 
            => _WriteLineColorized( message, ConsoleColor.Yellow );

        public override void Warning( Exception ex )
            => Warning( ex.ToString() );


        private void _WriteLineColorized( string text, ConsoleColor foregroundColor )
        {
            _LockInvoke( () =>
            {
                ConsoleColor backForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine( text );
                Console.ForegroundColor = backForegroundColor;
            } );
        }
        
        private void _LockInvoke(Action act )
        {
            lock ( _mutex )
            {
                act();
            }
        }


        private static object _mutex = new object();
    }
}
