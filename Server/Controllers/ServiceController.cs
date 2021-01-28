using System;
using System.Diagnostics;

using Journal.Server.Infrastructure;

using Microsoft.AspNetCore.Mvc;

namespace Journal.Server.Controllers
{
    [ApiController
        , Route(ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)]
    public class ServiceController : Controller
    {
        public IActionResult GetStats()
        {
            DateTime startTime = _currentProcess.StartTime;
            return Json(new
            {
                startTime,
                workTime = DateTime.Now - startTime
            });
        }

        private static Process _currentProcess = Process.GetCurrentProcess();
    }
}
