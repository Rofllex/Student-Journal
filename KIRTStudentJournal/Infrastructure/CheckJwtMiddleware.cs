using KIRTStudentJournal.Database;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KIRTStudentJournal.Infrastructure
{
    public static class JwtUtils
    {
        /// <summary>
        /// Получить JWT токен из <see cref="IHeaderDictionary"/>
        /// </summary>
        /// <returns>
        /// Если не удалось считать JWT токен вернет <see cref="default"/>
        /// </returns>
        public static string GetJwtTokenFromHeaderDictionary(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary.ContainsKey("Authorization"))
            {
                string fullAuthorization = headerDictionary["Authorization"];
                if (fullAuthorization.IndexOf("Bearer") == 0)
                    return fullAuthorization.Remove(0, 7);
                return default;
            }
            else
                return default;
        }
    }

    /// <summary>
    /// Middleware для проверки токена из базы данных.
    /// Если токен не указан в заголовке(или указан в неверном формате) то пропускает дальше. Если токен указан верно и он истек то не пропустит дальше выдав ошибку. <see cref="StatusCodes.Status403Forbidden"/>
    /// </summary>
    public class CheckJwtMiddleware
    {
        public CheckJwtMiddleware()
        {

        }

        public async Task Invoke(HttpContext httpContext, Func<Task> next)
        {
            string jwtToken = JwtUtils.GetJwtTokenFromHeaderDictionary(httpContext.Request.Headers);
            if (jwtToken != default)
            {
                using var db = new DatabaseContext();
                var parsedToken = new ParsedJwtToken(jwtToken);
                var token = db.Tokens.Where(t => t.Sign == parsedToken.Sign && t.Payload == parsedToken.Payload).FirstOrDefault();
                if (token != null && token.ExpireDate <= DateTime.Now)
                {
                    db.Tokens.Remove(token);
                    await db.SaveChangesAsync();
                    token = null;
                }
                if (token == null)
                {
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    string errorJson = JsonConvert.SerializeObject(new Models.Error("Токен не найден или истек. Требуется авторизация"));
                    await httpContext.Response.WriteAsync(errorJson, System.Text.Encoding.UTF8);
                    return;
                }
            }
            await next.Invoke();
        }
    }
}
