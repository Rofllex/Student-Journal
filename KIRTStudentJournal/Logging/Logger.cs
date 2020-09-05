using System;
namespace KIRTStudentJournal.Logging
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
    
        public static Logger Instance { get; set; }
    }
}
