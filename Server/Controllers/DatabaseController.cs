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

namespace Journal.Server.Controllers
{
    [ApiController
        , Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)
        , Authorize]
    public class DatabaseController : Controller
    {
        private readonly JournalDbContext _dbContext; 

        public DatabaseController()
        {
            _dbContext = JournalDbContext.CreateContext();
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
                catch 
                {
                    return StatusCode(StatusCodes.Status400BadRequest);        
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Common.Models.WrongParametersError(new Dictionary<string, string>
                {
                    [nameof(count)] = count.ToString(),
                    [nameof(offset)] = offset.ToString()
                }));
            }
        }

       /// <summary>
       ///  Метод создания специальности.
       /// </summary>
       /// <param name="name">Название специальности</param>
       /// <param name="code">Код специальности</param>
       /// <param name="maxCourse">Максимальное кол-во курсов.</param>
       /// <returns></returns>
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
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
                if (specialty == default)
                {
                    specialty = new Specialty
                    {
                        Name = name
                        , Code = code
                        , MaxCourse = maxCourse
                    };
                    _dbContext.Specialties.Add(specialty);

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (System.Exception e)
                    {
                        Logging.Logger logInstance = Logging.Logger.Instance;
                        logInstance.Error($"{nameof(DatabaseController)}.{nameof(CreateSpecialty)} Ошибка добавления специальности в бд.\n" + e);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Json(specialty);
                }
                else
                    return Json(new RequestError("Данная специальность уже присутствует в бд"));
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
        /// Получение специальности по идентификатору.
        /// </summary>
        /// <param name="specialtyId">Идентификатор специальности</param>
        public async Task<IActionResult> GetSpecialtyById(
            [FromQuery(Name = "id")] int specialtyId)
        {
            return await Task.Run(() =>
            {
                Specialty specialty = _dbContext.Specialties.FirstOrDefault(s => s.Id == specialtyId);
                if (specialty != default)
                {
                    return (IActionResult)Json(specialty);
                }
                else
                    return Json(new RequestError("Специальность не найдена."));
            });
        }

        /// <summary>
        ///     Метод получения нескольких предметов.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public IActionResult GetSubjects( [FromQuery( Name = "count" )] int count = 10, [FromQuery( Name = "offset" )] int offset = 0 )
        {
            if (count > 0 && offset < 0)
            {
                int subjectsCount = _dbContext.Specialties.Count( );
                return Json( new
                {
                    subjects = _dbContext.Subjects.Skip( count ).Take( offset ).ToArray( ),
                    subjectsCount
                } );
            }
            else
            {
                return StatusCode( StatusCodes.Status400BadRequest , new Common.Models.WrongParametersError( new Dictionary<string , string>
                {
                    [nameof( count )] = count.ToString( ) ,
                    [nameof( offset )] = offset.ToString( )
                } ) );
            }
        }
        
        /// <summary>
        ///     Метод получения предмета по идентификатору.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public IActionResult GetSubjectById([FromQuery(Name = "subjectId")] int subjectId )
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if ( subjectId < 0 )
                return Json( new InvalidArgumentRequestError( nameof( subjectId ) ) );
            Subject subject = _dbContext.Subjects.FirstOrDefault( s => s.Id == subjectId );
            if ( subject != null )
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return Json( subject );
            }
            else
                return Json( new RequestError($"Предмет с идентификатором { subjectId } не найден") );
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
        public IActionResult CreateStudentGroup(
            [FromQuery(Name = "specialtyId")] int specialtyId
            , [FromQuery(Name = "curatorId")] int curatorId
            , [FromQuery(Name = "currentCourse")] int currentCourse
            , [FromQuery(Name = "subgroup")] int subgroup)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if ( specialtyId < 0 )
                return Json( new InvalidArgumentRequestError( nameof( specialtyId ) ) );
            if ( curatorId < 0 )
                return Json( new InvalidArgumentRequestError( nameof( curatorId ) ) );
            Specialty specialty = _dbContext.Specialties.FirstOrDefault( s => s.Id == specialtyId );
            if ( specialty != default )
            {
                User curator = _dbContext.Users.FirstOrDefault( u => u.Id == curatorId );
                if ( curator != default )
                {
                    StudentGroup studentGroup = new StudentGroup()
                    {
                        CuratorEnt = curator,
                        SpecialtyEnt = specialty,
                        CurrentCourse = currentCourse,
                        Subgroup = subgroup
                    };

                    _dbContext.Groups.Add( studentGroup );
                    _dbContext.SaveChanges();

                    Response.StatusCode = StatusCodes.Status200OK;
                    return Json( studentGroup );
                }
                else
                    return Json( new RequestError($"Пользователь с идентификатором {curatorId} не найден") );
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
        [Authorize( Roles = nameof( UserRole.Admin ) )]
        public IActionResult SetStudentGroup( 
            [FromQuery(Name = "studentId")] int studentId
            ,[FromQuery(Name = "groupId")] int groupId )
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if ( studentId < 0 )
                return Json( new InvalidArgumentRequestError( nameof( studentId ) ) );
            if ( groupId < 0 )
                return Json( new InvalidArgumentRequestError( nameof( groupId ) ) );
            
            Student student = _dbContext.Students.FirstOrDefault( s => s.UserId == studentId );
            if ( student != default )
            {
                StudentGroup group = _dbContext.Groups.FirstOrDefault( g => g.Id == groupId );
                if ( group != default )
                {
                    student.GroupEnt = group;
                    _dbContext.SaveChanges();
                    return StatusCode( StatusCodes.Status200OK );
                }
                else
                    return Json( new RequestError($"Группа с идентификатором {groupId} не найдена.") );
            }
            else
                return Json( new RequestError( $"Пользователь с идентификатором {studentId} не найден." ) ) ;
        }
    }
}
