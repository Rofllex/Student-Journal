using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using KIRTStudentJournal.Database;
using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using KIRTStudentJournal.Infrastructure;

namespace KIRTStudentJournal.Controllers.API
{
    [Controller]
    [Route(template: Infrastructure.API.CONTROLLER_ROUTE)]
    public class AccountController : Controller
    {
        /// <summary>
        /// Метод авторизации пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>
        /// Если пара логин-пароль будет неверная то вернет "неверный пароль".
        /// Если пользователь уже авторизирован, то вернет <see cref="StatusCodes.Status400BadRequest"/>
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromQuery(Name = "login")] string login, [FromQuery(Name = "pass")] string password)
        {
            if (JwtUtils.GetJwtTokenFromHeaderDictionary(Request.Headers) == default)
            {
                string pwdHash = getHashFromString(password);
                Account account;
                using var db = new DatabaseContext();
                account = db.Accounts.Where(a => a.Login == login && a.PasswordHash == pwdHash).FirstOrDefault();
                if (account != default)
                {
                    #region commented
                    //List<Claim> claims = new List<Claim>()
                    //{
                    //    new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    //    new Claim(ClaimsIdentity.DefaultRoleClaimType, Enum.GetName(typeof(Role), account.Role)),
                    //};
                    //var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    //var jwtTokenExpireDate = DateTime.Now.Add(TimeSpan.FromHours(1));
                    //var jwtToken = new JwtSecurityToken(
                    //    issuer: Jwt.ISSUER,
                    //    audience: Jwt.AUDIENCE,
                    //    notBefore: DateTime.Now,
                    //    claims: claimsIdentity.Claims,
                    //    expires: jwtTokenExpireDate,
                    //    signingCredentials: new SigningCredentials(Jwt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    //var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                    #endregion
                    var encodedJwt = createToken(login, account.Role, out DateTime jwtTokenExpireDate);
                    db.Tokens.Add(new JwtToken()
                    {
                        Token = encodedJwt,
                        ExpireDate = jwtTokenExpireDate,
                        GrantedFor = account
                    });
                    await db.SaveChangesAsync();
                    dynamic response = new ExpandoObject();
                    response.token = encodedJwt;
                    response.role = Enum.GetName(typeof(Role), account.Role);
                    return Json(response);
                }
                else
                    return new Models.Error("Неверный логин или пароль").ToActionResult();
            }
            else
                return StatusCode(StatusCodes.Status400BadRequest);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var claim = User.Claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType).FirstOrDefault();
            if (claim != default)
            {
                string jwtToken = JwtUtils.GetJwtTokenFromHeaderDictionary(Request.Headers);
                if (jwtToken != default)
                {
                    using var db = new DatabaseContext();
                    var token = db.Tokens.Where(t => t.Token == jwtToken).FirstOrDefault();
                    if (token != default)
                    {
                        db.Tokens.Remove(token);
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                }
                else
                    Logging.Logger.Instance.Error($"Попытка удаления токена которого нет в заголовке. IP: \"{ HttpContext.Connection.RemoteIpAddress }\"");
            }
            else
                Logging.Logger.Instance.Error($"Авторизация пройдена, но клайма с логином нет. IP: \"{ HttpContext.Connection.RemoteIpAddress }\" ");
            return Forbid();
        }
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            string tokenString = JwtUtils.GetJwtTokenFromHeaderDictionary(Request.Headers);
            if (tokenString != default)
            {
                using var db = new DatabaseContext();
                var token = db.Tokens.Where(t => t.Token == tokenString).FirstOrDefault();
                string newToken = createToken(token.GrantedFor.Login, token.GrantedFor.Role, out DateTime expireDate);
                token.Token = newToken;
                await db.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            else
                return StatusCode(StatusCodes.Status401Unauthorized);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult TestAuth()
        {
            return Content("success!");
        }


        private static readonly HashAlgorithm _hashAlgorithm = SHA256.Create();
        /// <summary>
        /// Получить токен MD5 из строки
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private string getHashFromString(string inputString)
        {
            byte[] buffer = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            string str = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
                str += buffer[i].ToString("x2");
            return str;
        }
        /// <summary>
        /// Создание JWT токена.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="role"></param>
        /// <param name="expireDate"></param>
        /// <returns></returns>
        private string createToken(string login, Role role, out DateTime expireDate)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Enum.GetName(typeof(Role), role))
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            expireDate = DateTime.Now.AddHours(Jwt.HOURS_LIFETIME);
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                    issuer: Jwt.ISSUER,
                    audience: Jwt.AUDIENCE,
                    notBefore: DateTime.Now,
                    claims: claimsIdentity.Claims,
                    expires: expireDate,
                    signingCredentials: new SigningCredentials(Jwt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string jwtTokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return jwtTokenString;
        }

    }
}
