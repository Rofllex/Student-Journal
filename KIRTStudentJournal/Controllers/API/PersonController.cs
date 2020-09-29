using KIRTStudentJournal.Database;
using KIRTStudentJournal.Database.Journal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIRTStudentJournal.Shared.Models;

namespace KIRTStudentJournal.Controllers.API
{
    [Controller]
    [Route(Infrastructure.API.CONTROLLER_ROUTE)]
    public class PersonController : Controller
    {
        public PersonController()
        {

        }
        
        [Authorize]
        public IActionResult GetMe()
        {
            string login = User.Claims.FirstOrDefault(c => c.Type == Infrastructure.Jwt.DEFAULT_LOGIN_TYPE)?.Value;
            if (login != null)
            {
                Person person;
                using (var db = new DatabaseContext())
                    person = db.Persons.FirstOrDefault(p => p.Account.Login == login);
                if (person != null)
                    return Json(new PersonModel(person.FirstName, person.LastName, person.Patronymic, person.PhoneNumber));
                else
                {
                    Logging.Logger.Instance.Error($"Пользователь авторизован хотя Person в бд не найден. Login: \"{ login }\"");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                Logging.Logger.Instance.Error("Неверный клайм с пустым логином");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
