#nullable enable

namespace Journal.ClientLib
{
    /// <summary>
    ///     Исключение при неудачном подключении.
    /// </summary>
    public class ConnectFaillureException : ExecuteQueryException
    {
        public ConnectFaillureException(string url) : base( $"Исключение при попытке подключения к серверу \"{ url }\"." ) { }
    }
}
