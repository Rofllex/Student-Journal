using System;
using System.Diagnostics;

#nullable enable

namespace Journal.ClientLib
{
    /// <summary>
    ///     Базовый класс исключения при выполнении запроса.
    /// </summary>
    [Serializable,
        DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ExecuteQueryException : Exception
    {
        public ExecuteQueryException(string message, string? responseString = null) : base(message) => (ResponseString) = responseString;

        public ExecuteQueryException(string message, string? responseString = null, Exception? innerException = null) : base(message, innerException)
        {
            ResponseString = responseString;
        }

        public ExecuteQueryException(string message, int statusCode, string? responseString, Exception? innerException) : this(message, responseString, innerException)
        {
            StatusCode = statusCode;
        }


        /// <summary>
        ///     Ответ от сервера.
        ///     Может быть null.
        /// </summary>
        public string? ResponseString { get; protected set; }

        /// <summary>
        ///     HTTP статус-код ответа от сервера.
        /// </summary>
        public int? StatusCode { get; protected set; }

        private string GetDebuggerDisplay()
        {
            string statusCodeString = StatusCode.HasValue ? StatusCode.Value.ToString() : "null";
            return $"Исключение при выполнении запроса StatusCode: { statusCodeString }. Message: \"{ Message }\"";
        }
    }
}
