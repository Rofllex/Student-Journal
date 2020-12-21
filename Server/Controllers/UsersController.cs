using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Journal.Server.Database;
using System.Linq;
using Journal.Common.Models;

namespace Journal.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        public UsersController()
        {
            _dbContext = JournalDbContext.CreateContext();
        }

        public IActionResult CreateStudent(string login, string password, string firstName, string surname, string lastName, string phoneNumber)
        {
            throw new System.NotImplementedException();
            
            if (_dbContext.Users.FirstOrDefault(u => u.Login == login) == default)
            {

            }
            else
                return Json(new RequestError("Аккаунт с таким логином уже занят"));
        }

        private readonly JournalDbContext _dbContext;
    }
}
