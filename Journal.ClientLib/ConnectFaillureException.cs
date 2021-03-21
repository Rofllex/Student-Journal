using System;

#nullable enable

namespace Journal.ClientLib
{
    /// <summary>
    ///     Исключение при неудачном подключении.
    /// </summary>
    public sealed class ConnectFaillureException : ExecuteQueryException
    {
        public ConnectFaillureException(string url, Exception? innerException = null) : base( $"Исключение при попытке подключения к серверу \"{ url }\".", null, innerException)  
        {
        
        }

        public ConnectFaillureException(Uri uri, Exception? innerException = null) : this (uri.ToString(), innerException)
        {
        }
    }
}
