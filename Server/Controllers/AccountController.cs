using System;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Journal.Common.Models;
using Journal.Server.Database;
using Journal.Common.Entities;
using Journal.Server.Security;
using Journal.Logging;

#nullable enable

namespace Journal.Server.Controllers
{
    [ApiController
        , Route(Infrastructure.ApiControllersRouting.API_CONTROLLER_DEFAULT_ROUTE)]
    public class AccountController : Controller
    {
        private readonly JournalDbContext _dbContext;

        public AccountController()
        {
            _dbContext = JournalDbContext.CreateContext();
        }

        
        /// <summary>
        /// Метод авторизации пользователя.
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public Task<IActionResult> Auth([FromQuery(Name = "login")] string login, [FromQuery(Name = "password")] string password)
        {
            return Task.Run(() =>
            {
                if (!User.Identity.IsAuthenticated)
                {
                    User? user = _AuthenticateUser(login, password);
                    if (user != null)
                    {
                        Logger.Instance.Info($"Пользователь: { user.Login } авторизован");

                        string token = _GenerateJWT(user, out DateTime tokenExpire),
                                refreshToken = _GenerateRefreshToken(user, token, out DateTime refreshTokenExpire);
                        return (IActionResult)Json(new
                        {
                            token,
                            tokenExpire,
                            refreshToken,
                            refreshTokenExpire,
                            user
                        });
                    }
                    else
                        return Unauthorized(new RequestError("Неверный логин или пароль"));
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest);
            });
        }

        /// <summary>
        /// Метод смены пароля.
        /// </summary>
        /// <param name="oldPassword">Старый пароль</param>
        /// <param name="newPassword">Новый пароль</param>
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromQuery(Name = "oldPassword")] string oldPassword, [FromQuery(Name = "newPassword")] string newPassword)
        {
            if (!string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(newPassword))
            {
                if (oldPassword != newPassword)
                {
                    User user = _GetUserFromClaims();
                    if (user != null)
                    {
                        if (user.PasswordHash == Hash.GetFromString(oldPassword))
                        {
                            user.PasswordHash = Hash.GetFromString(newPassword);
                            user.PasswordChanged = DateTime.Now;
                            await _dbContext.SaveChangesAsync();
                            string token = _GenerateJWT(user, out DateTime tokenExpire),
                                    refreshToken = _GenerateRefreshToken(user, token, out DateTime refreshTokenExpire);
                            return Json(new
                            {
                                token,
                                tokenExpire,
                                refreshToken,
                                refreshTokenExpire,
                            });

                            //return _CreateJWTActionResult(token, tokenExpire, refreshToken, refreshTokenExpire, role: user.URole.ToString());
                        }
                        else
                            return Json(new RequestError("Неверный старый пароль."));
                    }
                    else
                        return Json(new RequestError("Аккаунт недоступен."));
                }
                else
                    return Json(new RequestError("Новый пароль не может соответствовать старому."));
            }
            else
                return Json(new RequestError($"Поле \"{nameof(oldPassword)}\" или \"{nameof(newPassword)}\" было пустым"));
        }


        /// <summary>
        /// Метод обновления основного токена.
        /// </summary>
        /// <param name="refToken"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> RefreshToken( [FromQuery( Name = "refreshToken" )] string refToken )
        {
            return await Task.Run( () =>
             {
                 User user = _GetUserFromClaims();
                 JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                 
                 if ( true /* user.RefreshToken == refToken*/)
                 {
                     string token = _GenerateJWT( user, out DateTime tokenExpire ),
                             refreshToken = _GenerateRefreshToken( user, token, out DateTime refreshTokenExpire );
                     return (IActionResult)StatusCode(StatusCodes.Status501NotImplemented);
                     //return _CreateJWTActionResult( token, tokenExpire, refreshToken, refreshTokenExpire, user.URole.ToString());
                 }
                 else
                 {
                     Response.StatusCode = StatusCodes.Status400BadRequest;
                     return Unauthorized( new RequestError( "Неверный refreshToken." ) );
                 }
             } );
        }

        private bool _IsUserAuthenticated()
            => User.Claims.FirstOrDefault(c => c.Type == JwtTokenOptions.NAME_TYPE) != default;
        
        /// <summary>
        /// Метод получения пользователя из клаймов
        /// Данный метод следует вызывать только когда у метода есть атрибут <see cref="AuthorizeAttribute"/> иначе ебнет, охуеешь
        /// </summary>
        private User _GetUserFromClaims()
        {
            Claim loginClaim = User.Claims.FirstOrDefault(c => c.Type == JwtTokenOptions.NAME_TYPE);
            Debug.Assert(loginClaim != null);
            return _dbContext.Users.FirstOrDefault(u => u.Login == loginClaim.Value);
        }

        /// <summary>
        /// Метод аутентификации пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>
        /// Вернет null если пользователь не аутентифицирован
        /// </returns>
        private User? _AuthenticateUser(string login, string password)
        {
            string pwdHash = Hash.GetFromString(password, Encoding.UTF8);
            return _dbContext.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == pwdHash);
        }

        /// <summary>
        /// Метод создания JWT токена.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="expireDate">Дата истечения токена</param>
        private string _GenerateJWT(User user, out DateTime expireDate)
        {
            List<Claim> claims = new List<Claim>() 
            { 
                new Claim(JwtTokenOptions.NAME_TYPE, user.Login) 
            };
            
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (user.IsInRole(role))
                    claims.Add(new Claim(JwtTokenOptions.ROLE_TYPE, role.ToString()));
            }

            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims: claims,
                authenticationType: "Token",
                nameType: JwtTokenOptions.NAME_TYPE,
                roleType: JwtTokenOptions.ROLE_TYPE);
            
            DateTime now = DateTime.Now;
            expireDate = now.Add(AuthOptions.JWT_TOKEN_LIFETIME);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claimsIdentity.Claims,
                notBefore: now,
                expires: expireDate,
                signingCredentials: new SigningCredentials(key: AuthOptions.GetSymmetricSecurityKey(), algorithm: SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        /// <summary>
        /// Создать токен обновления.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="token">Основной JWT токен</param>
        /// <param name="refreshTokenExpire">Дата истечения токена</param>
        private string _GenerateRefreshToken(User user, string token, out DateTime refreshTokenExpire) 
        {
            // TODO: переписать алгоритм получения refresh token`а, лучше чтобы не зависел от основного токена.
            refreshTokenExpire = DateTime.Now.Add(AuthOptions.JWT_REFRESH_TOKEN_LIFETIME);
            return Hash.GetFromString(string.Concat(token.Substring(token.Length - 16, 16), ".", user.Login));
        }
    }
}
