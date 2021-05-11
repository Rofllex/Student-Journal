using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using Journal.Server.Database;
using Journal.Common.Entities;
using Journal.Common.Models;
using Journal.Server.Utils;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Journal.Server.Controllers
{
    [ApiController
        , Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)
        , Authorize]
    public class DatabaseController : Controller
    {

        public DatabaseController()
        {
            _dbContext = JournalDbContext.CreateContext();
        }


        /// <summary>
        ///  Метод создания специальности.
        /// </summary>
        /// <param name="name">Название специальности</param>
        /// <param name="code">Код специальности</param>
        /// <param name="maxCourse">Максимальное кол-во курсов.</param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))
            , HttpPost]
        public async Task<IActionResult> CreateSpecialty(
              [FromQuery(Name = "name")] string name
            , [FromQuery(Name = "code")] string code
            , [FromQuery(Name = "maxCourse")] int maxCourse)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (!string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(code)
                && maxCourse > 0 && maxCourse <= 4)
            {
                Specialty specialty = _dbContext.Specialties.Where(s => s.Name == name)
                                                            .FirstOrDefault();
                if (specialty != default)
                    return Json(new RequestError("Данная специальность уже присутствует в бд"));

                int[] subjectIds;
                try
                {
                    JToken body = await this.ReadJsonBody();
                    if (body.Type != JTokenType.Array)
                        throw new JsonSerializationException();
                    subjectIds = body.ToObject<int[]>();
                }
                catch (JsonSerializationException)
                {
                    return Json(new RequestError("Неверное тело запроса"));
                }

                IEnumerable<Subject> subjs;
                if (subjectIds.Length > 0)
                    subjs = _dbContext.Subjects.Where(s => subjectIds.Contains(s.Id));
                else
                    subjs = Enumerable.Empty<Subject>();

                specialty = new Specialty(name, code, maxCourse, subjs);
                _dbContext.Specialties.Add(specialty);

                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Logging.Logger logInstance = Logging.Logger.Instance;
                    logInstance.Error($"{nameof(DatabaseController)}.{nameof(CreateSpecialty)} Ошибка добавления специальности в бд.\n" + e);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                Response.StatusCode = StatusCodes.Status200OK;
                return Json(specialty);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new WrongParametersError(new Dictionary<string, string>
                {
                    [nameof(name)] = name,
                    [nameof(code)] = code,
                    [nameof(maxCourse)] = maxCourse.ToString()
                }));
            }
        }


        /// <summary>
        ///     Получение специальностей.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public IActionResult GetSpecialties([FromQuery(Name = "count")] int count = 10, [FromQuery(Name = "offset")] int offset = 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (count >= 1 && offset >= 0)
            {
                //int specialtiesCount = _dbContext.Specialties.Count();
                try
                {
                    Specialty[] specialties = _dbContext.Specialties.Skip(offset)
                                                                    .Take(count)
                                                                    .Include(s => s.Subjects)
                                                                    .ToArray();
                    Response.StatusCode = StatusCodes.Status200OK;
                    return Json(new
                    {
                        specialties,
                        count
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new WrongParametersError(new Dictionary<string, string>
                {
                    [nameof(count)] = count.ToString(),
                    [nameof(offset)] = offset.ToString()
                }));
            }
        }

        /// <summary>
        /// Получение специальности по идентификатору.
        /// </summary>
        /// <param name="specialtyId">Идентификатор специальности</param>
        public async Task<IActionResult> GetSpecialtyById([FromQuery(Name = "id")] int specialtyId)
        {
            return await Task.Run(() =>
            {
                Specialty specialty = _dbContext.Specialties.Include(s => s.Subjects).FirstOrDefault(s => s.Id == specialtyId);
                if (specialty != default)
                {
                    return (IActionResult)Json(specialty);
                }
                else
                    return Json(new RequestError("Специальность не найдена."));
            });
        }

        /// <summary>
        ///     Установить предмет для специальности.
        /// </summary>
        /// <param name="specialtyId">Идентификатор специальности</param>
        /// <param name="subjectId">Идентификатор предмета</param>
        /// <returns></returns>
        public async Task<IActionResult> SetSpecialtySubject([FromQuery(Name = "specialtyId")] int specialtyId, [FromQuery(Name = "subjectId")] int subjectId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (specialtyId < 0)
                return Json(new RequestError("Идентификатор специальности не может быть меньше 0"));
            else if (subjectId < 0)
                return Json(new RequestError("Идентификатор предмета не может быть меньше 0"));

            Specialty specialty = _dbContext.Specialties.Include(s => s.Subjects).FirstOrDefault(s => s.Id == specialtyId);
            if (specialty == default)
                return Json(new RequestError($"Специальность с идентификатором { specialtyId } не найдена"));
            Subject subject = specialty.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject != default)
                return Json(new RequestError($"Предмет \"{ subject.Name }\" уже добавлен в специальность"));

            subject = _dbContext.Subjects.FirstOrDefault(s => subjectId == s.Id);
            if (subject == default)
                return Json(new RequestError($"Предмет с идентификатором { subjectId } не найден"));
            specialty.Subjects.Add(subject);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        ///     Убрать предмет из специальности.
        /// </summary>
        /// <param name="specialtyId">Идентификатор специальности</param>
        /// <param name="subjectId">Идентификатор предмета</param>
        /// <returns></returns>
        public async Task<IActionResult> RemoveSpecialtySubject([FromQuery(Name = "specialtyId")] int specialtyId, [FromQuery(Name = "subjectId")] int subjectId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (specialtyId < 0)
                return Json(new RequestError("Идентификатор специальности не может быть меньше 0"));
            else if (subjectId < 0)
                return Json(new RequestError("Идентификатор предмета не может быть меньше 0"));

            Specialty specialty = _dbContext.Specialties.Include(s => s.Subjects).FirstOrDefault(s => s.Id == specialtyId);
            if (specialty == default)
                return Json(new RequestError($"Специальность с идентификатором { specialtyId } не найдена"));
            Subject subject = specialty.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject == default)
                return Json(new RequestError($"Предмет с идентификатором { subjectId } не добавлен в специальность"));
            specialty.Subjects.Remove(subject);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        ///     Получить предметы по специальности
        /// </summary>
        /// <param name="specialtyId">Идентификатор специальности</param>
        public Task<IActionResult> GetSpecialtySubjects([FromQuery(Name = "specialtyId")] int specialtyId)
        {
            return Task.Run(() =>
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                if (specialtyId < 0)
                    return Json(new RequestError("Идентификатор специальности не может быть меньше 0"));
                Specialty specialty = _dbContext.Specialties.Include(s => s.Subjects).FirstOrDefault(s => s.Id == specialtyId);
                if (specialty == default)
                    return Json(new RequestError($"Специальность с идентификатором { specialtyId } не найдена"));
                Response.StatusCode = StatusCodes.Status200OK;
                return (IActionResult)Json(specialty.Subjects);
            });
        }

        /// <summary>
        ///     Создание нового предмета.
        /// </summary>
        /// <param name="name">Название предмета</param>
        /// <param name="specialtyId">Идентификатор специальности</param>
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult CreateSubject([FromQuery(Name = "name")] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new InvalidArgumentRequestError(nameof(name)));
            }
            Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Name == name);
            if (subject != null)
                return Json(new RequestError($"Предмет с именем \"{ name }\" уже присутствует"));
            subject = new Subject(name);
            _dbContext.Subjects.Add(subject);
            _dbContext.SaveChanges();
            Response.StatusCode = StatusCodes.Status200OK;
            return Json(subject);
        }

        /// <summary>
        ///     Метод получения нескольких предметов.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public IActionResult GetSubjects([FromQuery(Name = "count")] int count = 10, [FromQuery(Name = "offset")] int offset = 0)
        {
            if (count > 0 && offset <= 0)
            {
                IQueryable<Subject> queryable = _dbContext.Subjects.Skip(offset)
                                                                    .Take(count);
                return Json(new
                {
                    subjects = queryable.ToArray(),
                    subjectsCount = queryable.Count()
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new WrongParametersError(new Dictionary<string, string>
                {
                    [nameof(count)] = count.ToString(),
                    [nameof(offset)] = offset.ToString()
                }));
            }
        }

        /// <summary>
        ///     Метод получения предмета по идентификатору.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public IActionResult GetSubjectById([FromQuery(Name = "subjectId")] int subjectId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (subjectId < 0)
                return Json(new InvalidArgumentRequestError(nameof(subjectId)));
            Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject != null)
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return Json(subject);
            }
            else
                return Json(new RequestError($"Предмет с идентификатором { subjectId } не найден"));
        }

        /// <summary>
        ///     Метод создания группы студентов.
        /// </summary>
        /// <param name="specialtyId"></param>
        /// <param name="curatorId"></param>
        /// <param name="currentCourse"></param>
        /// <param name="subgroup"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateStudentGroup(
            [FromQuery(Name = "specialtyId")] int specialtyId
            , [FromQuery(Name = "curatorId")] int curatorId
            , [FromQuery(Name = "currentCourse")] int currentCourse
            , [FromQuery(Name = "subgroup")] int subgroup)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (specialtyId < 0)
                return Json(new InvalidArgumentRequestError(nameof(specialtyId)));
            if (curatorId < 0)
                return Json(new InvalidArgumentRequestError(nameof(curatorId)));

            Specialty specialty = _dbContext.Specialties.FirstOrDefault(s => s.Id == specialtyId);
            if (specialty != default)
            {
                User curator = _dbContext.Users.FirstOrDefault(u => u.Id == curatorId);
                if (curator != default)
                {
                    if (_dbContext.Groups.FirstOrDefault(g => g.SpecialtyId == specialtyId && g.CurrentCourse == currentCourse && g.Subgroup == subgroup) != null)
                        return Json(new RequestError("Данная группа уже присутствует в системе."));

                    StudentGroup studentGroup = new StudentGroup(specialty, currentCourse, subgroup, new List<Student>(), curator);
                    _dbContext.Groups.Add(studentGroup);
                    await _dbContext.SaveChangesAsync();

                    Response.StatusCode = StatusCodes.Status200OK;
                    return Json(studentGroup);
                }
                else
                    return Json(new RequestError($"Пользователь с идентификатором {curatorId} не найден"));
            }
            else
                return Json(new RequestError($"Специальность с идентификатором {specialtyId} не найдена."));

        }

        /// <summary>
        ///     Установить группу для студента.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Admin))]
        public IActionResult SetStudentGroup(
            [FromQuery(Name = "studentId")] int studentId
            , [FromQuery(Name = "groupId")] int groupId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (studentId < 0)
                return Json(new InvalidArgumentRequestError(nameof(studentId)));
            if (groupId < 0)
                return Json(new InvalidArgumentRequestError(nameof(groupId)));

            Student student = _dbContext.Students.FirstOrDefault(s => s.UserId == studentId);
            if (student != default)
            {
                StudentGroup group = _dbContext.Groups.FirstOrDefault(g => g.Id == groupId);
                if (group != default)
                {
                    student.GroupEnt = group;
                    _dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK);
                }
                else
                    return Json(new RequestError($"Группа с идентификатором {groupId} не найдена."));
            }
            else
                return Json(new RequestError($"Пользователь с идентификатором {studentId} не найден."));
        }

        /// <summary>
        ///     Получить группу студентов по идентификатору.
        /// </summary>
        /// <param name="groupId">Идентификатор группы</param>
        /// <returns></returns>
        public IActionResult GetGroupById([FromQuery(Name = "groupId")] int groupId)
        {
            StudentGroup group = _dbContext.Groups.Include(g => g.SpecialtyEnt).Include(g => g.CuratorEnt).FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new RequestError($"Группа с идентификатором { groupId } не найдена"));
            }

            return Json(group);
        }

        /// <summary>
        ///     Получение групп студентов
        /// </summary>
        public IActionResult GetGroups([FromQuery(Name = "offset")] int offset, [FromQuery(Name = "count")] int count)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (count < 0)
                return Json(new RequestError("Параметр count не может быть меньше 0"));
            if (offset < 0)
                return Json(new RequestError("Параметр offset не может быть меньше 0"));

            StudentGroup[] groups = _dbContext.Groups.Include(g => g.SpecialtyEnt)
                                                     .Include(g => g.CuratorEnt)
                                                     .Skip(offset)
                                                     .Take(count)
                                                     .ToArray();
            Response.StatusCode = StatusCodes.Status200OK;
            return Json(new
            {
                count = groups.Length,
                groups
            });
        }

        /// <summary>
        ///     Получить список групп по специальности.
        /// </summary>
        /// <param name="specialtyId">Идентификатор специальности</param>
        public IActionResult GetGroupsBySpecialty([FromQuery(Name = "specialtyId")] int specialtyId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (specialtyId < 1)
                return Json(new RequestError("Параметр specialtyId не может быть меньше 1"));
            StudentGroup[] groups = _dbContext.Groups.Include(g => g.SpecialtyEnt).Where(g => g.SpecialtyId == specialtyId).ToArray();
            Response.StatusCode = StatusCodes.Status200OK;
            return Json(new
            {
                count = groups.Length,
                groups
            });
        }
        
        /// <summary>
        ///     Получить список студентов в группе.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IActionResult GetStudentsInGroup([FromQuery(Name = "groupId")] int groupId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (groupId < 1)
                return Json(new RequestError("Параметр groupId не может быть меньше 1"));
            Student[] students = _dbContext.Students.Where(s => s.GroupId == groupId).Include(s => s.UserEnt).ToArray();
            Response.StatusCode = StatusCodes.Status200OK;
            return Json(new
            {
                count = students.Length,
                students
            });
        }

        private readonly JournalDbContext _dbContext;
    }
}
