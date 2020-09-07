using Newtonsoft.Json;

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
    }
}
