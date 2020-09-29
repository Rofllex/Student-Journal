using System;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using KIRTStudentJournal.Models;
using KIRTStudentJournal.Database;
using KIRTStudentJournal.Infrastructure;
using KIRTStudentJournal.Shared.Models;

namespace KIRTStudentJournal.Controllers.API
{
    [Controller]
    [Route(template: Infrastructure.API.CONTROLLER_ROUTE)]
    public class AccountController : Controller
    {
        public AccountController()
        {

        }

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
                    ParsedJwtToken parsedJwtToken = new ParsedJwtToken(tokenString);
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
                    AccountAuthorized accountModel = new AccountAuthorized(account.Id, new TokenModel(tokenString, jwtToken.RefreshToken, jwtToken.ExpireDate), new RoleModel());
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
                    AccountAuthorized accountModel = new AccountAuthorized(token.GrantedFor.Id, new TokenModel(token.FullToken, token.RefreshToken, expireDate), new RoleModel(token.GrantedFor.Role));
                    return Json(accountModel);
                }
                else
                    return new Error("Неверный refresh токен").ToActionResult();
            }
            else
                return new Error("Токен не найден").ToActionResult();
        }

        /// <summary>
        /// Получить список ролей.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public Task<IActionResult> GetRoles()
        {
            dynamic response = new ExpandoObject();
            Type roleType = typeof(Role);
            Array valuesArray = Enum.GetValues(roleType),
                  namesArray = Enum.GetNames(roleType);
            dynamic[] roles = new dynamic[valuesArray.Length];
            for (int i = 0; i < roles.Length; i++)
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
        /// Метод изменения пароля.
        /// </summary>
        /// <param name="newPassword">Новый пароль</param>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword([FromQuery(Name = "oldPass")] string oldPass, [FromQuery(Name = "newPass")] string newPassword)
        {
            string login = User.Claims.FirstOrDefault(c => c.Type == Jwt.DEFAULT_LOGIN_TYPE)?.Value;
            if (login != null)
            {
                using (var db = new DatabaseContext())
                {
                    Account account = db.Accounts.FirstOrDefault(a => a.Login == login && a.PasswordHash == Hash.GetHashFromString(oldPass));
                    if (account != null)
                    {
                        var otherTokens = db.Tokens.Where(t => t.GrantedFor.Login == login && t.FullToken != JwtUtils.GetJwtTokenFromHeaderDictionary(Request.Headers));
                        if (otherTokens.Count() > 0)
                            db.Tokens.RemoveRange(otherTokens);
                        account.PasswordHash = Hash.GetHashFromString(newPassword);
                        await db.SaveChangesAsync();
                        return StatusCode(StatusCodes.Status200OK);
                    }
                    else
                        return Json(new Error("Неверный пароль"));
                }
            }
            else
            {
                Logging.Logger.Instance.Error("Пользователь авторизирован, хотя клайм логина null.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Метод получения аккаунтов
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult GetAccounts([FromQuery(Name = "offset")] int offset, [FromQuery(Name = "count")] int count)
        {
            List<Account> accountsList;
            using (var db = new DatabaseContext())
                accountsList = db.Accounts.Skip(offset).Take(count).ToList();
            var accounts = accountsList.ConvertAll(a => new AccountModel(a.Id, new RoleModel(a.Role)));
            return Json(new
            {
                count = accounts.Count,
                accounts
            });
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
                new Claim(Jwt.DEFAULT_LOGIN_TYPE, login),
                new Claim(Jwt.DEFAULT_ROLE_TYPE, Enum.GetName(typeof(Role), role))
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
