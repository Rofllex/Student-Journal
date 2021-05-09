using System;
using System.Diagnostics;


#nullable enable

namespace Journal.ClientLib
{
    /// <summary>
    ///     Исключение при неверном статус коде ответа от сервера.
    /// </summary>
    [Serializable,
        DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class WrongStatusCodeException : ExecuteQueryException
    {
        public int HttpStatusCode { get; set; }

        public int StatusCodeInt => HttpStatusCode;

        internal WrongStatusCodeException( int statusCode, string? message = null, string? response = null ) : base( message, response ) => HttpStatusCode = statusCode;

        internal WrongStatusCodeException(int statusCode) : this(statusCode, $"Неверный код ответа({(int)statusCode}).") { }

        private string GetDebuggerDisplay()
            => $"Неверный код ответа от сервера {HttpStatusCode}({StatusCodeInt})";
        
    }
}
