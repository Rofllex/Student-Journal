using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KIRTStudentJournal.Models
{
    public class Error
    {
        [JsonProperty("error_message")]
        public string Message { get; set; }

        public Error(string message)
        {
            Message = message;
        }

        public IActionResult ToActionResult()
        {
            return new JsonResult(this);
        }
    }
}
