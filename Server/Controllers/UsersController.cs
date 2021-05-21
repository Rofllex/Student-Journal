using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        ///     Получение информации о текущем пользователе.
        /// </summary>
        public Task<IActionResult> GetMe()
        {
            return Task.Run(() =>
            {
                Claim nameClaim = User.Claims.FirstOrDefault(c => c.Type == Security.JwtTokenOptions.NAME_TYPE);
                Debug.Assert(nameClaim != null);

                User user = _dbContext.Users.FirstOrDefault(u => u.Login == nameClaim.Value);
                return (IActionResult)Json(this.GetUserFromClaims(_dbContext.Users));
            });
        }

        public async Task<IActionResult> GetUser([FromQuery(Name = "id")] int userId)
        {
            return await Task.Run(() =>
           {
               User user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
               if (user != default)
               {
                   return Json(user);
               }
               else
                   return Json(new RequestError($"Пользователь с идентификатором { userId } не найден."));
           });
        }

        public async Task<IActionResult> GetTeacher([FromQuery(Name = "teacherId")] int teacherId)
        {
            return await Task.Run(() =>
            {
                if (teacherId < 0)
                    return Json(new RequestError($"Параметр { nameof(teacherId) } не может быть меньше 0"));

                Teacher teacher = _dbContext.Teachers.FirstOrDefault(t => t.UserId == teacherId);
                if (teacher == null)
                    return Json(new RequestError($"Преподаватель с идентификатором { teacherId } не найден"));
                return Json(teacher);
            });
        }

        public Task<IActionResult> GetTeachers([FromQuery(Name = "offset")] int offset
                                                    , [FromQuery(Name = "count")] int count)
            => Task.Run<IActionResult>(()=>
            {
                return Json(_dbContext.Users.Where(u => u.URole.Value.HasFlag(UserRole.Teacher)).Skip(offset).Take(count));
            });


        [Authorize(Roles = nameof(UserRole.Admin))]
        public Task<IActionResult> GetUsers([FromQuery(Name = "offset")] int offset, [FromQuery(Name = "count")] int count)
        {
            return Task.Run(() =>
           {
               User[] users = _dbContext.Users.Skip(offset)
                                               .Take(count)
                                               .ToArray();
               return (IActionResult)Json(new
               {
                   users,
                   count = users.Length
               });
           });
        }

        public Task<IActionResult> GetUsersCount()
        {
            return Task.Run(() =>
            {
                return (IActionResult)Json(_dbContext.Users.Count());
            });
        }

        public IActionResult GetStudent([FromQuery(Name = "id")] int userId)
        {
            Student student = _dbContext.Students.FirstOrDefault(s => s.UserId == userId);
            if (student != null)
            {
                return Json(student);
            }
            else
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new RequestError($"Пользователь с идентификатором {userId} не является студентом"));
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateUser([FromQuery(Name = "login")] string login
                                                    , [FromQuery(Name = "password")] string password
                                                    , [FromQuery(Name = "role")] UserRole role
                                                    , [FromQuery(Name = "firstName")] string firstName
                                                    , [FromQuery(Name = "surname")] string surname = null
                                                    , [FromQuery(Name = "lastName")] string lastName = null
                                                    , [FromQuery(Name = "phoneNumber")] string phoneNumber = null)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (!_VerifyUserFields(_dbContext.Users, login, password, firstName, surname, out IActionResult result))
                return result;
            User user = _CreateUser(login, Security.Hash.GetFromString(password), role, firstName, surname, lastName, phoneNumber);
            _dbContext.Users.Add(user);
            
            foreach (UserRole containsRole in Common.Extensions.EnumExtensions.GetContainsFlags(role))
            {
                switch (containsRole)
                {
                    case UserRole.Student:
                        Student student = new Student(user);
                        _dbContext.Students.Add(student);
                        break;
                    case UserRole.Teacher:
                        Teacher teacher = new Teacher(user);
                        _dbContext.Teachers.Add(teacher);
                        break;
                }
            }
            
            await _dbContext.SaveChangesAsync();
            Response.StatusCode = StatusCodes.Status200OK;
            return Json(user);
        }

        /// <summary>
        ///     Метод создания студента.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="surname"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateStudent(
            [FromQuery(Name = "login")] string login
            , [FromQuery(Name = "password")] string password
            , [FromQuery(Name = "firstName")] string firstName
            , [FromQuery(Name = "surname")] string surname
            , [FromQuery(Name = "lastName")] string lastName
            , [FromQuery(Name = "phoneNumber")] string phoneNumber
            , [FromQuery(Name = "groupId")] int groupId = -1)
        {
            // Значение по умолчанию.
            Response.StatusCode = StatusCodes.Status400BadRequest;

            if (!_VerifyUserFields(_dbContext.Users, login, password, firstName, surname, out IActionResult actionResult))
                return actionResult;

            StudentGroup group;
            if (groupId >= 0)
            {
                if ((group = _dbContext.Groups.FirstOrDefault(g=> g.Id == groupId)) == default)
                    return Json(new RequestError($"Группа с идентификатором {groupId} не найдена."));
            }
            else
                group = null;

            if (_dbContext.Users.FirstOrDefault(u => u.Login == login) != default)
                return Json(new RequestError($"Логин \"{ login }\" занят."));

            User user = _CreateUser(login, Security.Hash.GetFromString(password), UserRole.Student, firstName, surname, lastName, phoneNumber);
            _dbContext.Users.Add(user);

            Student student = new Student(user, group);
            _dbContext.Students.Add(student);

            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateTeacher(
              [FromQuery(Name = "login")] string login
            , [FromQuery(Name = "password")] string password
            , [FromQuery(Name = "firstName")] string firstName
            , [FromQuery(Name = "surname")] string surname
            , [FromQuery(Name = "lastName")] string lastName = null
            , [FromQuery(Name = "phoneNumber")] string phoneNumber = null)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (!_VerifyUserFields(_dbContext.Users, login, password, firstName, surname, out IActionResult actionResult))
                return actionResult;
            
            User user = _CreateUser(login, Security.Hash.GetFromString(password), UserRole.Teacher, firstName, surname, lastName, phoneNumber);
            _dbContext.Users.Add(user);
            Teacher teacher = new Teacher(user);
            _dbContext.Teachers.Add(teacher);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> SetUserRole([FromQuery(Name = "userId")] int userId, [FromQuery(Name = "role")] UserRole role)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            
            User user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return Json(new RequestError($"Пользователь с идентификатором { userId } не найден"));
            user.URole = role;
            foreach (UserRole hasRoles in Common.Extensions.EnumExtensions.GetContainsFlags(role))
            {
                switch (hasRoles) 
                {
                    case UserRole.Teacher:
                        Teacher teacher = _dbContext.Teachers.FirstOrDefault(t => t.UserId == userId);
                        if (teacher == null)
                            _dbContext.Teachers.Add(new Teacher(user));
                        break;
                    case UserRole.Student:
                        if (_dbContext.Students.FirstOrDefault(s => s.UserId == userId) == null)
                            _dbContext.Students.Add(new Student(user));
                        break;
                }
            }

            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        private readonly JournalDbContext _dbContext;
    
        private bool _VerifyUserFields(DbSet<User> usersDbSet, string login, string password, string firstName, string surname, out IActionResult actionResult)
        {
            actionResult = null;
            if (string.IsNullOrWhiteSpace(login) || login.Length < 5)
                actionResult = Json(new InvalidArgumentRequestError(nameof(login)));
            else if (string.IsNullOrWhiteSpace(password) || password.Length < 5)
                    actionResult = Json(new InvalidArgumentRequestError(nameof(password)));
            else if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 3)
                actionResult = Json(new InvalidArgumentRequestError(nameof(firstName)));
            else if (string.IsNullOrWhiteSpace(surname) || surname.Length < 3)
                actionResult = Json(new InvalidArgumentRequestError(nameof(surname)));
            else if (usersDbSet.FirstOrDefault(u => u.Login == login) != default)
                actionResult = Json(new RequestError("Аккаунт с таким логином уже занят"));
            return actionResult == null;
        }
    
        private User _CreateUser(string login, string passwordHash, UserRole role, string firstName, string surname, string lastName, string phoneNumber)
        {
            User user = new User(firstName, surname, login, passwordHash, role);
            if (!string.IsNullOrWhiteSpace(lastName))
                user.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
                user.PhoneNumber = phoneNumber;
            return user;
        }
    }
}
