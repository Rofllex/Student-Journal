using System;
using System.Diagnostics;

namespace Journal.Common.Models
{
    /// <summary>
    /// Ошибка запроса.
    /// </summary>
    [DebuggerDisplay( "{" + nameof( GetDebuggerDisplay ) + "(),nq}" )]
    public class RequestError
    {
        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Message { get; set; }
        
        public RequestError( string message)
        {
            Message = message;
        }

        public RequestError() 
        {
        }

        /// <summary>
        /// Выбросить исключение <see cref="RequestErrorException"/>
        /// </summary>
        public void Throw()
            => Throw(null);

        /// <summary>
        /// Выбросить исключение RequestErrorException с сообщением.
        /// </summary>
        public void Throw(string message) 
            => throw CreateException(message);

        public override string ToString()
            => Message;

        /// <summary>
        /// Метод позволяющий получить исключение для методов <see cref="Throw"/> и <see cref="Throw(string)"/>
        /// </summary>
        /// <param name="message">Может быть null.</param>
        protected virtual RequestErrorException CreateException(string message)
            => new RequestErrorException(this, message);

        private string GetDebuggerDisplay()
            => Message;
    }

    /// <summary>
    /// Исключение при выполнении запроса.
    /// </summary>
    public class RequestErrorException : Exception
    {
        public RequestError Error { get; private set; }

        public RequestErrorException( RequestError error ) : this (error, default) { }

        public RequestErrorException( RequestError error, string message) : base (message)
        {
            Error = error;
        }
    }
}
