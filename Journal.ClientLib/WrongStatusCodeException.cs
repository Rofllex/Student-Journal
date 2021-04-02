using System;
using System.Diagnostics;

using Leaf.xNet;

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
        public HttpStatusCode HttpStatusCode { get; set; }

        public int StatusCodeInt => ( int )HttpStatusCode;

        internal WrongStatusCodeException( HttpStatusCode statusCode, string? message = null, string? response = null ) : base( message, response ) => HttpStatusCode = statusCode;

        internal WrongStatusCodeException(HttpStatusCode statusCode) : this(statusCode, $"Неверный код ответа({(int)statusCode}).") { }

        private string GetDebuggerDisplay()
            => $"Неверный код ответа от сервера {HttpStatusCode}({StatusCodeInt})";
        
    }
}
