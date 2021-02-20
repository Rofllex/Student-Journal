using System;
using System.Diagnostics;

using Leaf.xNet;

#nullable enable

namespace Journal.ClientLib
{
    [Serializable,
        DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class WrongStatusCodeException : ExecuteQueryException
    {
        public HttpStatusCode StatusCode { get; set; }

        public int StatusCodeInt => ( int )StatusCode;

        internal WrongStatusCodeException( HttpStatusCode statusCode, string? message = null, string? response = null ) : base( message, response ) => StatusCode = statusCode;

        internal WrongStatusCodeException(HttpStatusCode statusCode) : this(statusCode, $"Неверный код ответа({(int)statusCode}).") { }

        private string GetDebuggerDisplay()
            => $"Неверный код ответа от сервера {StatusCode}({StatusCodeInt})";
        
    }
}
