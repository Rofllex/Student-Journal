using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Journal.Server.Database;
using Journal.Common.Models;
using Journal.Server.Infrastructure;
using Journal.Common.Entities;
using Microsoft.AspNetCore.Http;

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
        public Task<IActionResult> GetUsers([FromQuery(Name = "offset")] int offset, [FromQuery(Name = "count")] int count )
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
        [Authorize( Roles = nameof( UserRole.Admin ) )]
        public async Task<IActionResult> CreateStudent(
            [FromQuery( Name = "login" )] string login
            , [FromQuery( Name = "password" )] string password
            , [FromQuery( Name = "firstName" )] string firstName
            , [FromQuery( Name = "surname" )] string surname
            , [FromQuery( Name = "lastName" )] string lastName
            , [FromQuery( Name = "phoneNumber" )] string phoneNumber
            , [FromQuery(Name = "groupId")] int groupId = -1)
        {
            // Значение по умолчанию.
            Response.StatusCode = StatusCodes.Status400BadRequest;

            if ( !string.IsNullOrWhiteSpace( login ) && login.Length >= 5 )
            {
                if ( !string.IsNullOrWhiteSpace( password ) && password.Length >= 5 )
                {
                    if ( !string.IsNullOrWhiteSpace( firstName ) && firstName.Length >= 2 )
                    {
                        if ( !string.IsNullOrWhiteSpace( surname ) && surname.Length >= 2 )
                        {
                            if ( _dbContext.Users.FirstOrDefault( u => u.Login == login ) == default )
                            {
                                StudentGroup group;
                                if ( groupId >= 0 )
                                {
                                    // Присвоить group значение из бд и если оно null, то вернуть ошибку запроса.
                                    if ((group = _dbContext.Groups.Where( g => g.Id == groupId ).FirstOrDefault()) == default)
                                        return Json( new RequestError($"Группа с идентификатором {groupId} не найдена.") );
                                }
                                else
                                    group = null;
                                User user = new User( firstName, surname, login, Security.Hash.GetFromString( password ), UserRole.Student )
                                {
                                    LastName = lastName,
                                    PhoneNumber = phoneNumber
                                };

                                _dbContext.Users.Add( user );

                                Student student = new Student( user );
                                _dbContext.Students.Add( student );
                                
                                await _dbContext.SaveChangesAsync();
                                return StatusCode( StatusCodes.Status200OK );
                            }
                            else
                                return Json( new RequestError( "Аккаунт с таким логином уже занят" ) );
                        }
                        else
                            return Json( new InvalidArgumentRequestError( nameof( surname ) ) );
                    }
                    else
                        return Json( new InvalidArgumentRequestError( nameof( firstName ) ) );
                }
                else
                    return Json( new InvalidArgumentRequestError( nameof( password ) ) );
            }
            else
                return Json( new InvalidArgumentRequestError( nameof( login ) ) );
        }

        

        private readonly JournalDbContext _dbContext;
    }
}
