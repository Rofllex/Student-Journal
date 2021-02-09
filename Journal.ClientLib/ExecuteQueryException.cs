using System;
using System.Diagnostics;

#nullable enable

namespace Journal.ClientLib
{
    [Serializable,
        DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ExecuteQueryException : Exception
    {
        public ExecuteQueryException(string message) : base(message) { }

        private string GetDebuggerDisplay()
            => "Исключение при вызове метода";
    }
}
