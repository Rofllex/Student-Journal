﻿using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Journal.Server.Controllers
{
    [Controller]
    public class ViewController : Controller
    {
        public IActionResult Index()
        {
            return View("Pages/Index.cshtml");
        }
    }
}
