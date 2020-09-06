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
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Auth([FromQuery(Name = "login")] string login, [FromQuery(Name = "pass")] string password)
        {
            string pwdHash = getMd5FromString(password);
            Account account;
            using var db = new DatabaseContext();
            account = db.Accounts.Where(a => a.Login == login && a.PasswordHash == pwdHash).FirstOrDefault();
            if (account != default)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, Enum.GetName(typeof(Role), account.Role)),
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                var jwtTokenExpireDate = DateTime.Now.Add(TimeSpan.FromHours(1));
                var jwtToken = new JwtSecurityToken(
                    issuer: Jwt.ISSUER,
                    audience: Jwt.AUDIENCE,
                    notBefore: DateTime.Now,
                    claims: claimsIdentity.Claims,
                    expires: jwtTokenExpireDate,
                    signingCredentials: new SigningCredentials(Jwt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);
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

        private string getMd5FromString(string inputString)
        {
            var md5 = MD5.Create();
            byte[] buffer = Encoding.UTF8.GetBytes(inputString);
            return Convert.ToBase64String(md5.ComputeHash(buffer));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult TestAuth()
        {
            return Content("success!");
        }
    }
}
