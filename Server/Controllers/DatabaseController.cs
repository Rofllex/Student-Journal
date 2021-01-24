using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Journal.Server.Database;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Journal.Server.Controllers
{
    [ApiController
        , Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)
        , Authorize]
    public class DatabaseController : Controller
    {
        private readonly JournalDbContext dbContext; 

        public DatabaseController()
        {
            dbContext = JournalDbContext.CreateContext();
        }

        public IActionResult GetSpecialties( [FromQuery( Name = "count" )] int count = 10, [FromQuery( Name = "offset" )] int offset = 0)
        {
            if (count >= 1 && offset >= 0)
            {
                int specialtiesCount = dbContext.Specialties.Count( );
                Specialty[] specialties = dbContext.Specialties.Skip( offset ).Take( count ).ToArray( );
                return Json( new
                {
                    specialties ,
                    specialtiesCount
                } );
            }
            else
            {
                return StatusCode( StatusCodes.Status400BadRequest , new Common.Models.WrongParametersError( new Dictionary<string , string>
                {
                    [nameof( count )] = count.ToString(),
                    [nameof( offset )] = offset.ToString()
                } ) );
            }
        }

        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> CreateSpecialty(
            [FromQuery( Name = "name" )] string name
            , [FromQuery( Name = "code" )] string code
            , [FromQuery( Name = "maxCourse" )] int maxCourse )
        {
            if (!string.IsNullOrWhiteSpace( name )
                && !string.IsNullOrWhiteSpace( code )
                && maxCourse > 0 && maxCourse <= 4)
            {
                Specialty specialty = dbContext.Specialties.FirstOrDefault( s => s.Name == name || s.Code == code );
                if (specialty == default)
                {
                    specialty = new Specialty
                    {
                        Name = name
                        , Code = code
                        , MaxCourse = maxCourse
                    };
                    dbContext.Specialties.Add( specialty );

                    try
                    {
                        await dbContext.SaveChangesAsync();
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
                    return Json( new Common.Models.RequestError("Данная специальность уже присутствует в бд"));
            }
            else
            {
                return StatusCode( StatusCodes.Status400BadRequest , new Common.Models.WrongParametersError( new Dictionary<string , string>
                {
                    [nameof( name )] = name,
                    [nameof( code )] = code,
                    [nameof( maxCourse )] = maxCourse.ToString( )
                } ) );
            }
        }

        public IActionResult GetSubjects( [FromQuery( Name = "count" )] int count = 10, [FromQuery( Name = "offset" )] int offset = 0 )
        {
            if (count > 0 && offset < 0)
            {
                int subjectsCount = dbContext.Specialties.Count( );
                return Json( new
                {
                    subjects = dbContext.Subjects.Skip( count ).Take( offset ).ToArray( ),
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
