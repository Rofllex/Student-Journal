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
using System.Diagnostics;
using KIRTStudentJournal.Models;
using System.Collections;

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
                string pwdHash = Hash.GetHashFromString(password);
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
                    var tokenString = createToken(login, account.Role, out DateTime jwtTokenExpireDate);
                    var parsedJwtToken = new ParsedJwtToken(tokenString);
                    string refreshToken = Jwt.CreateRefreshToken(account, tokenString);
                    var jwtToken = new JwtToken()
                    {
                        Header = parsedJwtToken.Header,
                        Payload = parsedJwtToken.Payload,
                        Sign = parsedJwtToken.Sign,
                        RefreshToken = refreshToken,
                        ExpireDate = jwtTokenExpireDate,
                        GrantedFor = account
                    };
                    db.Tokens.Add(jwtToken);
                    await db.SaveChangesAsync();
                    //dynamic response = new ExpandoObject();
                    //response.token = tokenString;
                    //response.role = Enum.GetName(typeof(Role), account.Role);
                    //response.role_id = (int)account.Role;
                    //response.refresh_token = refreshToken;
                    AccountModel accountModel = new AccountModel(account, jwtToken);
                    return Json(accountModel);
                }
                else
                    return new Error("Неверный логин или пароль").ToActionResult();
            }
            else
                return StatusCode(StatusCodes.Status400BadRequest);
        }
        
        /// <summary>
        /// Выход из системы и обнуление токена.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            Claim claim = User.Claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType).FirstOrDefault();
            if (claim != default)
            {
                string jwtToken = JwtUtils.GetJwtTokenFromHeaderDictionary(Request.Headers);
                if (jwtToken != default)
                {
                    using var db = new DatabaseContext();
                    ParsedJwtToken parsedJwtToken = new ParsedJwtToken(jwtToken);
                    var token = db.Tokens.Where(t => t.Sign == parsedJwtToken.Sign && t.Payload == parsedJwtToken.Payload).FirstOrDefault();
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
        
        /// <summary>
        /// Метод обновления токена по токену обновления.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>
        /// При каких-либо ошибках вернет <see cref="Error"/>
        /// При удачном результате вернет
        /// {
        ///     token: "",
        ///     refresh_token: "",
        ///     role: "";
        /// }
        /// </returns>
        [Authorize]
        public async Task<IActionResult> Refresh([FromQuery(Name = "refresh_token")] string refreshToken)
        {
            string jwtTokenString = JwtUtils.GetJwtTokenFromHeaderDictionary(Request.Headers);
            var parsedJwtToken = new ParsedJwtToken(jwtTokenString);
            Claim loginClaim = User.FindFirst(Jwt.DEFAULT_LOGIN_TYPE);
            using var db = new DatabaseContext();
            var token = db.Tokens.Where(t => t.GrantedFor.Login == loginClaim.Value).FirstOrDefault();
            if (token != null)
            {
                if (token.RefreshToken == refreshToken)
                {
                    string newToken = createToken(token.GrantedFor.Login, token.GrantedFor.Role, out DateTime expireDate);
                    parsedJwtToken.JwtToken = newToken;
                    refreshToken = Jwt.CreateRefreshToken(token.GrantedFor, newToken);
                    token.ExpireDate = expireDate;
                    token.Header = parsedJwtToken.Header;
                    token.Payload = parsedJwtToken.Payload;
                    token.Sign = parsedJwtToken.Sign;
                    token.RefreshToken = refreshToken;
                    await db.SaveChangesAsync();
                    AccountModel accountModel = new AccountModel(new RoleModel(token.GrantedFor.Role), new TokenModel(token));
                    return Json(accountModel);
                }
                else
                    return new Error("Неверный refresh токен").ToActionResult();
            }
            else
                return new Error("Токен не найден").ToActionResult();
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult TestAuth()
        {
            return Content("success!");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetRoles()
        {
            dynamic response = new ExpandoObject();
            Type roleType = typeof(Database.Role);
            Array valuesArray = Enum.GetValues(roleType),
                  namesArray = Enum.GetNames(roleType);
            dynamic[] roles = new dynamic[valuesArray.Length];
            for (int i = 0; i < roles.Length ; i++)
            {
                dynamic role = new ExpandoObject();
                role.name = namesArray.GetValue(i);
                role.id = valuesArray.GetValue(i);
                roles[i] = role;
            }
            response.roles = roles;
            return Json(response);
        }

        /// <summary>
        /// Создание JWT токена.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="role"></param>
        /// <param name="expireDate"></param>
        /// <returns></returns>
        private string createToken(string login, Database.Role role, out DateTime expireDate)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(Jwt.DEFAULT_LOGIN_TYPE, login),
                new Claim(Jwt.DEFAULT_ROLE_TYPE, Enum.GetName(typeof(Database.Role), role))
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", Jwt.DEFAULT_LOGIN_TYPE, Jwt.DEFAULT_ROLE_TYPE);
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
