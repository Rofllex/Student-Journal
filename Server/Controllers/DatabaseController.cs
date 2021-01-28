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

        public IActionResult GetSpecialties([FromQuery(Name = "count")] int count = 10, [FromQuery(Name = "offset")] int offset = 0)
        {
            if (count >= 1 && offset >= 0)
            {
                //int specialtiesCount = _dbContext.Specialties.Count();
                try
                {
                    Specialty[] specialties = _dbContext.Specialties.Skip(offset)
                                                                    .Take(count)
                                                                    .Include(s => s.Subjects)
                                                                    .ToArray();
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

       
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Teacher))]
        public async Task<IActionResult> CreateSpecialty(
              [FromQuery(Name = "name")] string name
            , [FromQuery(Name = "code")] string code
            , [FromQuery(Name = "maxCourse")] int maxCourse)
        {
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
                        var logInstance = Logging.Logger.Instance;
                        logInstance.Error($"{nameof(DatabaseController)}.{nameof(CreateSpecialty)} Ошибка добавления специальности в бд.\n" + e);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    return Json(specialty);
                }
                else
                    return Json(new Common.Models.RequestError("Данная специальность уже присутствует в бд"));
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Common.Models.WrongParametersError(new Dictionary<string, string>
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
    }
}
