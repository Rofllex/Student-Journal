using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Journal.Server.Database;
using System.Security.Claims;
using Journal.Server.Infrastructure;
using Journal.Common.Models;
using Journal.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;

/*
 * Студент не должен видеть оценки других студентов.
 * Родители могут видеть оценки лишь своих детей.
 * Администратор может просматривать все оценки.
 */

namespace Journal.Server.Controllers
{
    [ApiController
        , Route(ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)
        , Authorize]
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
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
        public async Task<IActionResult> Paste(int studentId, int subjectId, GradeLevel level, string reason)
        {
            return await Task.Run(() =>
            {
                Student student = _dbContext.Students.Where(s => s.UserId == studentId)
                                                     .Include(s => s.UserEnt)
                                                     .FirstOrDefault();
                if (student != null && student.UserEnt.IsInRole(UserRole.Student))
                {
                    Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjectId);
                    if (subject != null)
                    {
                        User user = this.GetUserFromClaims(_dbContext);
                        Grade grade = new Grade(user, subject, student, level, reason: reason);
                        _dbContext.Grades.Add(grade);
                        _dbContext.SaveChanges();
                        return Json(grade);
                    }
                    else
                        return Json(new RequestError($"Предмет с идентификатором {subjectId} не найден!"));
                }
                else
                    return Json(new RequestError("Пользователь не найден или не является студентом."));
            });
        }
        
        // not implemented
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
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
            throw new NotImplementedException();
        }

        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> GetGradesForMe([FromQuery(Name = "subjectId")] int subjId)
        {
            return await Task.Run(() =>
            {
                User user = this.GetUserFromClaims(_dbContext);
                if (user.IsInRole(UserRole.Student))
                {
                    Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjId);
                    if (subject != default)
                    {
                        Grade[] grades = _dbContext.Grades.Where(g => g.SubjectId == subjId && g.StudentId == user.Id)
                                                          .ToArray();
                        return Json(new 
                        {
                            grades = grades,
                            count = grades.Length
                        });
                    }
                    else
                        return Json(new RequestError($"Предмет с идентификатором { subjId } не найден"));
                }
                else
                    return Json(new RequestError("Вы не являетесь студентом"));
            });
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher) + "," + nameof(UserRole.StudentParent))]
        public Task<IActionResult> GetGrades([FromQuery(Name = "studentId")] int studentId, [FromQuery(Name = "subjectId")]int subjectId)
        {
            return Task.Run(() =>
            {
                User currentUser = this.GetUserFromClaims(_dbContext);
                Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjectId);
                if (subject != default)
                {
                    Student student;
                    if (currentUser.IsInRole(UserRole.Admin) || currentUser.IsInRole(UserRole.Teacher))
                    {
                        student = _dbContext.Students.Include(s => s.UserId).FirstOrDefault(s => s.UserId == studentId);
                    }
                    else if (currentUser.IsInRole(UserRole.StudentParent))
                    {
                        Parent parent = _dbContext.Parents.Include(p => p.ChildStudentEnts).FirstOrDefault(p => p.UserId == currentUser.Id);
                        if (parent != null)
                        {
                            // Если выбранный студент не является чадом аккаунта родителя.
                            if ((student = parent.ChildStudentEnts.FirstOrDefault(s => s.UserId == studentId)) == null)
                                return (IActionResult)Json(new RequestError($"Студент с идентификатором \"{ studentId }\" не является вашим ребенком."));
                        }
                        else
                        {
                            Logging.Logger.Instance.Error($"{nameof(GradesController)}.{nameof(GetGrades)} Запись в таблице ${nameof(_dbContext.Parents)} не найдена, хотя у User({currentUser.Id}) роль {currentUser.URole}");
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
                    }
                    Grade[] grades = _GetGrades(studentId, subjectId);
                    return Json(new
                    {
                        grades = grades,
                        count = grades.Length
                    });
                }
                else
                    return Json(new RequestError($"Предмет с идентификатором { subjectId } не найдена."));
            });
        }

        private readonly JournalDbContext _dbContext;
    
        private Grade[] _GetGrades(int studentId, int subjectId)
            => _dbContext.Grades.Where(g => g.StudentId == studentId && g.SubjectId == subjectId).ToArray();

        private Grade[] _GetGrades(int studentId, int subjectId, DateTime startDate, DateTime endDate)
            => _dbContext.Grades.Where(g => g.StudentId == studentId 
                                            && g.SubjectId == subjectId 
                                            && g.Timestamp >= startDate 
                                            && g.Timestamp < endDate).ToArray();
    }
}
