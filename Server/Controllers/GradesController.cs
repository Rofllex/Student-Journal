using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

using Journal.Server.Database;
using System.Security.Claims;
using Journal.Server.Infrastructure;
using Journal.Common.Models;
using Journal.Common.Entities;

namespace Journal.Server.Controllers
{
    [ApiController]
    [Route(ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)]
    [Authorize]
    public class GradesController : Controller
    {
        
        public GradesController()
        {
            _dbContext = JournalDbContext.CreateContext();
        }
        

        /// <summary>
        /// Выставить оценку.
        /// </summary>
        /// <param name="studentId">Идентификатор студента</param>
        /// <param name="level">Уровень оценки</param>
        /// <returns></returns>
        public IActionResult Paste(int studentId, GradeLevel level, string reason)
        {
            throw new System.NotImplementedException();

            //User user;
            //if ((user = this.GetUserFromClaims(dbContext)) != null
            //    && dbContext.IsUserInRole(user, Role.TEACHER_ROLE_NAME))
            //{
            //    var student = dbContext.Users.Where(u => u.Id == studentId).FirstOrDefault();
            //    if (student != default)
            //    {
            //        //var pasteGrade = new JournalLogick.EfSingleStudentLogick(student, dbContext);
            //        throw new System.NotImplementedException();
            //    }
            //    else
            //    {
            //        return Json(new RequestError($"Пользователь с идентификатором \"{ studentId }\" не является студентом"));
            //    }
            //} 
            //else
            //    return Unauthorized();

        }

        /// <summary>
        /// Множественное выставление оценок.
        /// Главное, чтобы студенты были из одной группы.
        /// </summary>
        /// <param name="studentsId"></param>
        /// <param name="level"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public IActionResult PasteMultiple(int[] studentsId, GradeLevel level, string reason)
        {
            

            throw new System.NotImplementedException();
        }

        private readonly JournalDbContext _dbContext;
    }
}
