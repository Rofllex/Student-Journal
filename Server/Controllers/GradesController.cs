using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using Journal.Server.Database;
using Journal.Server.Infrastructure;
using Journal.Common.Models;
using Journal.Common.Entities;
using Journal.Server.Utils;

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
        ///     Выставить оценку.
        /// </summary>
        /// <param name="studentId">Идентификатор студента</param>
        /// <param name="level">Уровень оценки</param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
        public async Task<IActionResult> Paste([FromQuery(Name = "studentId")] int studentId
                                             , [FromQuery(Name = "subjectId")] int subjectId
                                             , [FromQuery(Name = "level")] GradeLevel level
                                             , [FromQuery(Name = "timestamp")] string timestamp = null
                                             , [FromQuery(Name = "reason")] string reason = null)
        {
            return await Task.Run(() =>
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                DateTime gradeDate;
                if (!string.IsNullOrWhiteSpace(timestamp))
                {
                    try
                    {
                        gradeDate = DateTime.Parse(timestamp);
                        if (!_CheckGradeTimestamp(gradeDate))
                            throw new Exception();
                    }
                    catch
                    {
                        return Json(new RequestError($"Неверная дата."));
                    }
                }
                else
                    gradeDate = DateTime.Now;
                Grade grade = _dbContext.Grades.FirstOrDefault(g => g.StudentId == studentId
                                                                    && g.SubjectId == subjectId
                                                                    && g.Timestamp.Year == gradeDate.Year
                                                                    && g.Timestamp.Month == gradeDate.Month
                                                                    && g.Timestamp.Day == gradeDate.Day);
                if (grade != null)
                    return Json(new RequestError($"Оценка уже выставлена"));

                Student student = _dbContext.Students.Where(s => s.UserId == studentId)
                                                     .Include(s => s.UserEnt)
                                                     .FirstOrDefault();
                if (student != null && student.UserEnt.IsInRole(UserRole.Student))
                {
                    Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjectId);
                    if ( subject != null )
                    {
                        User user = this.GetUserFromClaims( _dbContext.Users );
                        grade = new Grade( user, subject, student, level, timestamp: gradeDate, reason: reason );
                        _dbContext.Grades.Add( grade );
                        _dbContext.SaveChanges();
                        Response.StatusCode = StatusCodes.Status200OK;
                        return Json( grade );
                    }
                    else
                        return Json( new InvalidArgumentRequestError( nameof(subjectId) ) );
                }
                else
                    return Json(new RequestError($"Пользователь с идентификатором {studentId} не найден или не является студентом."));
            });
        }

        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))
            , HttpPost]
        /// <summary>
        ///     Множественное выставление оценок.
        ///     Главное, чтобы студенты были из одной группы.
        /// </summary>
        /// <param name="studentsId"></param>
        /// <param name="level"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<IActionResult> PasteMultiple([FromQuery(Name = "subjectId")] int subjectId
                                                     , [FromQuery(Name = "gradeLevel")] GradeLevel level
                                                     , [FromQuery(Name = "timestamp")] string? timestamp = null
                                                     , [FromQuery(Name = "reason")] string reason = null)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (!Request.ContentType.Contains("application/json"))
                return Json(new RequestError("Неверный тип контента. Не содержит \"application/json\""));

            DateTime gradesTimeStamp;
            if (!string.IsNullOrWhiteSpace(timestamp))
            {
                try
                {
                    gradesTimeStamp = DateTime.Parse(timestamp);
                    if (!_CheckGradeTimestamp(gradesTimeStamp))
                        throw new Exception();
                }
                catch 
                {
                    return Json(new RequestError("Неверная дата выставления оценок"));
                }
            }
            else
                gradesTimeStamp = DateTime.Now;

            Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject == null)
                return Json(new RequestError($"Предмет с идентификатором \"{ subjectId }\" не найдена"));
            
            if (!((GradeLevel[])Enum.GetValues(typeof(GradeLevel))).Contains(level))
                return Json(new RequestError("Неверный тип оценки"));
            int[] studentIds;
            try
            {
                JToken jsonBody = await this.ReadJsonBody();
                if (jsonBody.Type != JTokenType.Array)
                    throw new JsonSerializationException();
                studentIds = jsonBody.ToObject<int[]>();
            }
            catch (JsonSerializationException) 
            {
                return Json(new RequestError("Ошибка при десериализации тела запроса"));
            }

            IQueryable<Student> query = _dbContext.Students.Where(s => studentIds.Contains(s.UserId));
            if (query.Count() != studentIds.Length)
                return Json(new RequestError("Некоторые студенты не были найдены."));

            var currentUser = this.GetUserFromClaims(_dbContext.Users);

            Grade[] grades = query.ToArray().Select(student => new Grade(currentUser, subject, student, level, DateTime.Now, reason)).ToArray();
            await _dbContext.Grades.AddRangeAsync(grades);
            await _dbContext.SaveChangesAsync();

            return Json(grades);
        }

        /// <summary>
        ///     Получить оценки для текущего пользователя.
        /// </summary>
        /// <param name="subjId">Идентификатор предмета</param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Student))]
        public async Task<IActionResult> GetGradesForMe([FromQuery(Name = "subjectId")] int subjId)
        {
            return await Task.Run(() =>
            {
                User user = this.GetUserFromClaims(_dbContext.Users);
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

        /// <summary>
        ///     Получить оценки для студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента</param>
        /// <param name="subjectId">Идентификатор предмета</param>
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher) + "," + nameof(UserRole.StudentParent))]
        public  Task<IActionResult> GetGrades([FromQuery(Name = "studentId")] int studentId, [FromQuery(Name = "subjectId")]int subjectId)
        {
            return Task.Run(() =>
            {
                User currentUser = this.GetUserFromClaims(_dbContext.Users);
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

        /// <summary>
        ///     Получить оценки за месяц.
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <param name="groupId">Идентификатор группы</param>
        /// <param name="subjectId">Идентификатор предмета</param>
        [Authorize]
        public Task<IActionResult> GetMonthGrades([FromQuery(Name ="year")] int year
                                                , [FromQuery(Name = "month")] int month
                                                , [FromQuery(Name = "groupId")] int groupId
                                                , [FromQuery(Name = "subjectId")] int subjectId)
        {
            return Task.Run(() => 
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                if (month < 1 || month > 12)
                    return (IActionResult)Json(new RequestError("Параметр \"month\" не может быть меньше 1 или больше 12"));
                int[] studentIds = _dbContext.Students.Where(s => s.GroupId == groupId).Select(s => s.UserId).ToArray();
                if (studentIds.Length == 0)
                    return Json(new RequestError($"Нет студентов в группе с идентификатором { groupId }"));
                
                Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjectId);
                if (subject == null)
                    return Json(new RequestError($"Пердмет с идентификатором { subjectId } не найден"));
                
                DateTime startDate = new DateTime(year, month, 1);
                int days = DateTime.DaysInMonth(year, month);
                DateTime endDate = startDate.AddDays(days - 1);
                Response.StatusCode = StatusCodes.Status200OK;
                Grade[] grades = _dbContext.Grades.Where(g => studentIds
                                                  .Contains(g.StudentId) && g.Timestamp >= startDate && g.Timestamp <= endDate && g.SubjectId == subjectId)
                                                  .Include(g => g.RatedByEnt)
                                                  .ToArray();
                var ratedByUsers = grades.GroupBy(g => g.RatedByEnt)
                                         .Select(g => g.Key)
                                         .ToList();
                return Json(new 
                { 
                    grades,
                    ratedByUsers
                });
            });
        }

        /// <summary>
        ///     Получить оценку за день
        /// </summary>
        /// <param name="date">Дата получения оценки</param>
        /// <param name="studentId">Идентификатор студента</param>
        /// <param name="subjectId">Идентификатор предмета</param>
        /// <returns></returns>
        public IActionResult GetGradeForDay([FromQuery(Name = "date")] string date,
                                                  [FromQuery(Name = "studentId")] int studentId,
                                                  [FromQuery(Name = "subjectId")] int subjectId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;

            DateTime dateTime;
            if (!DateTime.TryParse(date, out dateTime))
                return Json(new RequestError("Не удалось спарсить параметр \"date\""));

            if (studentId <= 0)
                return Json(new RequestError("Идентификатор студента не может быть меньше или равным 0"));

            if (subjectId <= 0)
                return Json(new RequestError("Идентификатор предмета не может быть меньше или равным 0"));

            Response.StatusCode = StatusCodes.Status200OK;
            Grade grade = _dbContext.Grades.FirstOrDefault(g => g.Timestamp >= dateTime
                                                                && g.Timestamp < dateTime.AddDays(1)
                                                                && g.StudentId == studentId
                                                                && g.SubjectId == subjectId);
            return Json(grade);    
        }

        private readonly JournalDbContext _dbContext;
    
        private Grade[] _GetGrades(int studentId, int subjectId)
            => _dbContext.Grades.Where(g => g.StudentId == studentId 
                                            && g.SubjectId == subjectId)
                                .ToArray();

        private Grade[] _GetGrades(int studentId, int subjectId, DateTime startDate, DateTime endDate)
            => _dbContext.Grades.Where(g => g.StudentId == studentId 
                                            && g.SubjectId == subjectId 
                                            && g.Timestamp >= startDate 
                                            && g.Timestamp < endDate)
                                .ToArray();

        private bool _CheckGradeTimestamp(DateTime timestamp)
            => timestamp > new DateTime(2019, 1, 1) && timestamp < DateTime.Now.AddDays(1);
    }
}
