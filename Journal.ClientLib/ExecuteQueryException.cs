using System;
using System.Diagnostics;

#nullable enable

namespace Journal.ClientLib
{
    [Serializable,
        DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ExecuteQueryException : Exception
    {
        /// <summary>
        ///     Ответ от сервера.
        ///     Может быть null.
        /// </summary>
        public string? ResponseString { get; protected set; }

        public ExecuteQueryException( string message, string? responseString = null ) : base( message ) => ( ResponseString ) = responseString;

        private string GetDebuggerDisplay()
            => "Исключение при вызове метода";
    }
}
