using System;

namespace KIRTStudentJournal.Logging
{
    public class CombinedLogger : Logger
    {
        private readonly Logger[] _loggers;
        public CombinedLogger(params Logger[] loggers)
        {
            _loggers = loggers;
        }

        public override void Cause      (string message)    => invokeLoggers(l => l.Cause(message));
        public override void Cause      (Exception e)       => invokeLoggers(l => l.Cause(e));
        public override void Error      (string message)    => invokeLoggers(l => l.Error(message));
        public override void Error      (Exception e)       => invokeLoggers(l => l.Error(e));
        public override void Fatal      (string message)    => invokeLoggers(l => l.Fatal(message));
        public override void Fatal      (Exception e)       => invokeLoggers(l => l.Fatal(e));
        public override void Info       (string message)    => invokeLoggers(l => l.Info(message));
        public override void Warning    (string message)    => invokeLoggers(l => l.Warning(message));
        public override void Warning    (Exception e)       => invokeLoggers(l => l.Warning(e));
        
        private void invokeLoggers(Action<Logger> act)
        {
            foreach (Logger logger in _loggers)
                act(logger);
        }
    }
}
