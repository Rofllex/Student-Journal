using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KIRTStudentJournal.NetLib.Models
{
    public class Error
    {
        [JsonProperty("error_message")]
        public string Message { get; private set; }

        public Error(string message)
        {
            Message = message;
        }

        public Error()
        {
        }

        public void Throw() => throw new RequestErrorException(this);

        public static bool IsError(JObject jObject) => jObject.ContainsKey("error_message");
    }
}
