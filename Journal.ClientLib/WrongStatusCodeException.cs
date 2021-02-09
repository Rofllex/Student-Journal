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
        
        public WrongStatusCodeException(HttpStatusCode statusCode, string message) : base (message) 
        {
            StatusCode = statusCode;
        }

        public WrongStatusCodeException(HttpStatusCode statusCode) : this(statusCode, $"Неверный код ответа({(int)statusCode}).") { }

        private string GetDebuggerDisplay()
            => $"Неверный код ответа от сервера {StatusCode}";
        
    }
}
