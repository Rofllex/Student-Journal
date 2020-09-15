using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIRTStudentJournal.Controllers
{
    [Controller]
    //[Route(template: "[controller]/[action=Index]")]
    public class DebugController : Controller
    {
        public IActionResult Index()
        {
            return View("Views/DebugView.cshtml");
        }
    }
}
