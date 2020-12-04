namespace Journal.Common.Models
{
    /// <summary>
    /// Ошибка запроса.
    /// </summary>
    public class RequestError
    {
        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Message { get; set; }
    
        public RequestError(string message)
        {
            Message = message;
        }

        public RequestError() 
        {
        }
    }
}
