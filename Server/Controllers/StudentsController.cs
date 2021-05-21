using Journal.Common.Entities;
using Journal.Server.Database;

using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Journal.Server.Controllers
{
    [ApiController
          , Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)
          , Authorize]
    public class StudentsController : Controller
    {
        public StudentsController() 
        {
            _dbContext = JournalDbContext.CreateContext();
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
        public Task<IActionResult> GetWithoutGroup([FromQuery(Name = "offset")] int offset = 0
                                                        , [FromQuery(Name = "count")] int count = 50)
            => Task.Run<IActionResult>(() =>
            {
                var students = _dbContext.Students.Where(s => s.GroupId == null).Skip(offset).Take(count).Include(s => s.UserEnt).ToArray();
                return Json(students);
            });
        

        private JournalDbContext _dbContext;
    }
}
