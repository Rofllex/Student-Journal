using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Journal.Server.Database;
using Journal.Common.Models;
using Journal.Server.Infrastructure;
using Journal.Common.Entities;

namespace Journal.Server.Controllers
{
    [Authorize
        , Route(ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)]
    public class UsersController : Controller
    {
        public UsersController()
        {
            _dbContext = JournalDbContext.CreateContext();
        }

        public Task<IActionResult> GetMe()
        {
            return Task.Run(() => 
            {
                User claimsUser = this.GetUserFromClaims(_dbContext);
                return (IActionResult)Json(claimsUser);
            });
        }

        public Task<IActionResult> GetUser([FromQuery(Name = "id")] int userId) 
        {
            
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
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
