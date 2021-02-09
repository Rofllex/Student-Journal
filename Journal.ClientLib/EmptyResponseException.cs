using System;
using System.Diagnostics;

#nullable enable

namespace Journal.ClientLib
{
    [Serializable,
        DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class EmptyResponseException : ExecuteQueryException
    {
        public EmptyResponseException() : base ("Пустой ответ от сервера") { }
        public EmptyResponseException(string message) : base(message) { }

        private string GetDebuggerDisplay()
            => "Пустой ответ от сервера";
    }
}
