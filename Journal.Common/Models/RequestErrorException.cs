
using System;
using System.Diagnostics;

namespace Journal.Common.Models
{
    /// <summary>
    /// Исключение при выполнении запроса.
    /// </summary>

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class RequestErrorException : Exception
    {
        public RequestErrorException(RequestError error) : this(error, message: default) { }

        public RequestErrorException(RequestError error, string message) : base(message)
        {
            Error = error;
        }

        public RequestErrorException(RequestError error, int statusCode, string message = null) : this(error, message)
        {
            StatusCode = statusCode;
        }

        public RequestError Error { get; private set; }
        public int? StatusCode { get; private set; }

        private string GetDebuggerDisplay()
        {
            string statusCode = StatusCode.HasValue ? StatusCode.Value.ToString() : "null";
            if (Error != null)
            {
                return $"Mesg: \"{ Error.Message }\". StatusCode: { statusCode }";
            }
            else
                return $"{Message}. StatusCode: { statusCode }";
        }
    }

}
