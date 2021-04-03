using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Journal.Server.Database;
using Journal.Common.Models;
using Journal.Server.Infrastructure;
using Journal.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
                User claimsUser = this.GetUserFromClaims(_dbContext);
                return (IActionResult)Json(claimsUser);
            });
        }

        public async Task<IActionResult> GetUser( [FromQuery( Name = "id" )] int userId )
        {
            return await Task.Run( () =>
            {
                User user = _dbContext.Users.FirstOrDefault( u => u.Id == userId );
                if ( user != default )
                {
                    return Json( user );
                }
                else
                    return Json( new RequestError( $"Пользователь с идентификатором { userId } не найден." ) );
            } );
        }

        [Authorize( Roles = nameof( UserRole.Admin ) )]
        public Task<IActionResult> GetUsers([FromQuery(Name = "offset")] int offset, [FromQuery(Name = "count")] int count)
        {
            return Task.Run( () =>
            {
                User[] users = _dbContext.Users.Skip( offset )
                                                .Take( count )
                                                .ToArray();
                return ( IActionResult )Json( new
                {
                    users,
                    count = users.Length
                } );
            } );
        }

        


        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
        public IActionResult GetStudent([FromQuery(Name = "id")] int userId)
        {
            Student student = _dbContext.Students.Include(s=>s.UserEnt).FirstOrDefault(s => s.UserId == userId);
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

            User user = _CreateUser(login, password, UserRole.Student, firstName, surname, lastName, phoneNumber);
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
            , [FromQuery(Name = "lastName")] string lastName
            , [FromQuery(Name = "phoneNumber")] string phoneNumber)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (!_VerifyUserFields(_dbContext.Users, login, password, firstName, surname, out IActionResult actionResult))
                return actionResult;
            
            User user = _CreateUser(login, password, UserRole.Teacher, firstName, surname, lastName, phoneNumber);
            _dbContext.Users.Add(user);
            Teacher teacher = new Teacher(user);
            _dbContext.Teachers.Add(teacher);
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
            else if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2)
                actionResult = Json(new InvalidArgumentRequestError(nameof(firstName)));
            else if (string.IsNullOrWhiteSpace(surname) || surname.Length < 2)
                actionResult = Json(new InvalidArgumentRequestError(nameof(surname)));
            else if (usersDbSet.FirstOrDefault(u => u.Login == login) != default)
                actionResult = Json(new RequestError("Аккаунт с таким логином уже занят"));
            return actionResult == null;
        }
    
        private User _CreateUser(string login, string password, UserRole role, string firstName, string surname, string lastName, string phoneNumber)
        {
            User user = new User(firstName, surname, login, password, role);
            if (!string.IsNullOrWhiteSpace(lastName))
                user.LastName = lastName;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
                user.PhoneNumber = phoneNumber;
            return user;
        }
    }
}
