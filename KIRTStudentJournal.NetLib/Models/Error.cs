using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KIRTStudentJournal.NetLib.Models
{
    /// <summary>
    /// Модель ошибки возвращаемой с сервера.
    /// </summary>
    public class Error
    {
        [JsonProperty("error_message")]
        public string Message { get; set; }

        public Error(string message)
        {
            Message = message;
        }

        public Error()
        {
        }

        public void Throw() => throw new RequestErrorException(this);

        public static bool IsError(JToken jToken) => jToken["error_message"] != null;
    }
}
