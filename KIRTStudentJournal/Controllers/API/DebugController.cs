using KIRTStudentJournal.Database;
using KIRTStudentJournal.Infrastructure;
using KIRTStudentJournal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIRTStudentJournal.Controllers.API
{
    [Controller]
    [Route(Infrastructure.API.CONTROLLER_ROUTE)]
    public class DebugController : Controller
    {
        public DebugController()
        {

        }

        [Authorize]
        public IActionResult GetRole()
        {
            string userLogin = User.Claims.FirstOrDefault(c => c.Type == Jwt.DEFAULT_LOGIN_TYPE)?.Value;
            if (userLogin != null)
            {
                Account account;
                using (var db = new DatabaseContext())
                    account = db.Accounts.FirstOrDefault(a => a.Login == userLogin);
                if (account != null)
                {
                    return Json(new RoleModel(account.Role));
                }
                else
                {
                    Logging.Logger.Instance.Error("Аккаунт не найден при авторизированном запросе!");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                Logging.Logger.Instance.Error($"Неверный логин пользователя в клаймах. From: \"{HttpContext.Connection.RemoteIpAddress}\" ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
