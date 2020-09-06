using Microsoft.AspNetCore.Mvc;

namespace KIRTStudentJournal.Models
{
    public class Error
    {
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
