using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Journal.Server.Database;
using Journal.Common.Models;



namespace Journal.Server.Controllers
{
    [ApiController
        , Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)
        , Authorize]
    public class SubjectsController : Controller
    {
        /// <summary>
        ///     Путь до папки с файлами учебного плана.
        /// </summary>
        private static readonly string  CurriculumDirectoryPath;

        static SubjectsController() 
        {
            CurriculumDirectoryPath = Path.Combine(Program.ExecutableDirectory, "curriculums");
        }


        public SubjectsController()
        {
            _dbContext = JournalDbContext.CreateContext();
        }

        /// <summary>
        ///     Загрузить учебный план
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadCurriculum([FromQuery(Name = "subjectId")] int subjectId
                                            , [FromQuery(Name = "specialtyId")] int specialtyId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (string.IsNullOrWhiteSpace(Request.ContentType) || !Request.ContentType.Contains("application/x-www-form-urlencoded"))
                return Json(new RequestError("Запрос содержит неверный заголовок типа контента. Он должен быть: \"application/x-www-form-urlencoded\""));
            else if (!Request.ContentLength.HasValue)
                return Json(new RequestError("Запрос не содержит contentLength"));

            Subject subject = _dbContext.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject == default)
                return Json(new RequestError($"Предмет с идентификатором \"{ subjectId }\" не найден"));
            
            Specialty specialty = _dbContext.Specialties.FirstOrDefault(s => s.Id == specialtyId);
            if (specialty == default)
                return Json(new RequestError($"Специальность с идентификатором \"{ specialty.Id }\" не найдена"));
            
            byte[] fileBytes;
            try
            {
                fileBytes = await _ReadBobyFileAsync(Request.Body, (int)Request.ContentLength.Value);
            }
            catch (Exception e)
            {
                _logger.Warning("Ошибка при чтении тела запроса");
                _logger.Warning(e);
                return Json(new RequestError("Ошибка при чтении тела запроса."));
            }
            
            string fileName = Guid.NewGuid().ToString("N");
            string fullFilePath = Path.Combine(CurriculumDirectoryPath, fileName);
            try
            {
                using (FileStream sw = System.IO.File.Create(fullFilePath, fileBytes.Length))
                    sw.Write(fileBytes, 0, fileBytes.Length);
            }
            catch (Exception e) 
            {
                var logger = Logging.Logger.Instance;
                logger.Error($"Ошибка записи файла учебного плана. Путь: \"{ fullFilePath }\"");
                logger.Error(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            Curriculum curriculum = new Curriculum(specialty, subject, fileName);
            _dbContext.Curriculums.Add(curriculum);
            await _dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK);
        }

        public IActionResult GetCurriculums([FromQuery(Name = "offset")] int offset,
                                            [FromQuery(Name = "count")] int count)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (offset < 0)
                return Json(new InvalidArgumentRequestError(nameof(offset)));
            if (count <= 0)
                return Json(new InvalidArgumentRequestError(nameof(count)));
            Response.StatusCode = StatusCodes.Status200OK;
            return Json(_dbContext.Curriculums.Skip(offset).Take(count));
        }

        public IActionResult GetCurriculumFile([FromQuery(Name = "curriculumId")] int curriculumId)
        {
            Curriculum curriculum = _dbContext.Curriculums.FirstOrDefault(c => c.Id == curriculumId);
            if (curriculum == null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new RequestError($"Учебный план с идентификатором { curriculumId } не найден"));
            }

            string curriculumFilePath = Path.Combine(CurriculumDirectoryPath, curriculum.FileName);
            if (!System.IO.File.Exists(curriculumFilePath))
            {
                this._logger.Error($"Файл учебного плана \"{ curriculumFilePath }\" не найден. ID: { curriculum.Id }");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return File(System.IO.File.OpenRead(curriculumFilePath), "application/x-www-form-urlencoded");
        }

        public async Task<IActionResult> RemoveCurriculum([FromQuery(Name = "curriculumId")] int curriculumId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            Curriculum curriculum = _dbContext.Curriculums.FirstOrDefault(c => c.Id == curriculumId);
            if (curriculum == default)
                return Json(new RequestError($"Учебный план с идентификатором \"{ curriculumId }\" не найден"));
            _dbContext.Curriculums.Remove(curriculum);
            await _dbContext.SaveChangesAsync();
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> ReplaceCurriculum([FromQuery(Name = "curriculumId")] int curriculumId)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            if (string.IsNullOrWhiteSpace(Request.ContentType) || !Request.ContentType.Contains("application/x-www-form-urlencoded"))
                return Json(new RequestError("Неверный тип контента"));
            if (!Request.ContentLength.HasValue)
                return Json(new RequestError("Заголовок не содержит длину контента"));
            Curriculum curriculum = _dbContext.Curriculums.FirstOrDefault(c => c.Id == curriculumId);
            if (curriculum == null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new RequestError($"Учебный план с идентификатором \"{ curriculumId }\" не найден"));
            }

            string fullFilePath = Path.Combine(CurriculumDirectoryPath, curriculum.FileName);
            if (!System.IO.File.Exists(fullFilePath))
            {
                _logger.Error($"Файл учебного плана по пути: \"{ fullFilePath }\" не найден");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            byte[] fileBytes;
            try
            {
                fileBytes = await _ReadBobyFileAsync(Request.Body, (int)Request.ContentLength.Value);
            }
            catch(Exception e)
            {
                var logger = Logging.Logger.Instance;
                logger.Warning("Не удалось прочитать тело запроса");
                logger.Warning(e);
                return Json(new RequestError("Не удалось прочитать тело запроса"));
            }

            try
            {
                System.IO.File.Delete(fullFilePath);
            }
            catch (Exception e)
            {
                _logger.Error($"Не удалось удалить файл учебного плана по пути: \"{ fullFilePath }\"");
                _logger.Error(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            try
            {
                using (FileStream fs = System.IO.File.Create(fullFilePath, fileBytes.Length))
                    fs.Write(fileBytes, 0, fileBytes.Length);
            }
            catch (Exception e) 
            {
                _logger.Error($"Не удалось записать файл учебного плана по пути: \"{ fullFilePath }\"");
                _logger.Error(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            return StatusCode(StatusCodes.Status200OK);
        }


        private JournalDbContext _dbContext;
        private Logging.Logger _logger => Logging.Logger.Instance;

        private async Task<byte[]> _ReadBobyFileAsync(Stream stream, int contentLength)
        {
            byte[] buffer = new byte[contentLength];
            int bytesRead = 0;
            do
            {
                bytesRead += await stream.ReadAsync(buffer, bytesRead, 1024);
            } while (bytesRead < contentLength);
            return buffer;
        }
    }
}
