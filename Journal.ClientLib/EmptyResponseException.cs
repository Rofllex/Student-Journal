using System;
using System.Diagnostics;

#nullable enable

namespace Journal.ClientLib
{
    [Serializable,
        DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class EmptyResponseException : ExecuteQueryException
    {
        public EmptyResponseException() : base ("Пустой ответ от сервера", null) { }
        public EmptyResponseException(string message) : base(message, null) { }

        private string GetDebuggerDisplay()
            => "Пустой ответ от сервера";
    }
}
