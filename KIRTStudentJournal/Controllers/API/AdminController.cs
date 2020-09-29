using KIRTStudentJournal.Models;
using KIRTStudentJournal.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIRTStudentJournal.Controllers.API
{
    [Controller]
    [Route(Infrastructure.API.CONTROLLER_ROUTE)]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public AdminController()
        {
        }

        public IActionResult RegisterAccount(string login, string password, Role role)
        {


            return new Error("Not implemented").ToActionResult();

            if (!string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(password))
            {

            }
        }
    }
}
