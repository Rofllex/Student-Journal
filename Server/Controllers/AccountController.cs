using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Journal.Server.Database;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Journal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly JournalDbContext dbContext;

        public AccountController()
        {
            dbContext = JournalDbContext.CreateContext();
        }

        public Task<IActionResult> Auth([FromQuery(Name = "login")] string login, [FromQuery(Name = "password")] string password)
        {
            return Task.Run(() =>
            {
                var user = AuthenticateUser(login, password);
                if (user != default)
                {
                    string token = GenerateJWT(user, out DateTime tokenExpire),
                            refreshToken = GenerateRefreshToken(user, token, out DateTime refreshTokenExpire);
                    return CreateJWTActionResult(token, tokenExpire, refreshToken, refreshTokenExpire, user.UserRole.ConvertAll(u => u.Role.Name).ToArray());
                }
                else
                    return Unauthorized();
            });
        }

        [Authorize]
        public async Task<IActionResult> ChangePassword([FromQuery(Name = "oldPassword")] string oldPassword, [FromQuery(Name = "newPassword")] string newPassword)
        {
            if (!string.IsNullOrWhiteSpace(oldPassword)
                && !string.IsNullOrWhiteSpace(newPassword)
                && oldPassword != newPassword)
            {
                User user = GetUserFromClaims();
                user.PasswordHash = Security.Hash.GetString(newPassword);
                user.PasswordChanged = DateTime.Now;
                await dbContext.SaveChangesAsync();
                string token = GenerateJWT(user, out DateTime tokenExpire),
                        refreshToken = GenerateRefreshToken(user, token, out DateTime refreshTokenExpire);
                return CreateJWTActionResult(token, tokenExpire, refreshToken, refreshTokenExpire, user.UserRole.ConvertAll(u => u.Role.Name).ToArray());    
            }
            else
                return Content("Неверные входые параметры");
        }

        [Authorize]
        public Task<IActionResult> RefreshToken([FromQuery(Name = "refreshToken")] string refToken)
        {
            return Task.Run(() =>
            {
                User user = GetUserFromClaims();
                if (user.RefreshToken == refToken)
                {
                    string token = GenerateJWT(user, out DateTime tokenExpire),
                            refreshToken = GenerateRefreshToken(user, token, out DateTime refreshTokenExpire);
                    return CreateJWTActionResult(token, tokenExpire, refreshToken, refreshTokenExpire, user.UserRole.ConvertAll(u => u.Role.Name).ToArray());
                }
                else
                {
                    return Unauthorized(new
                    {
                        message = "Неверный refreshToken"
                    });
                }
            });
        }

        /// <summary>
        /// Метод получения пользователя из клаймов
        /// </summary>
        /// <returns></returns>
        private User GetUserFromClaims()
        {
            Claim loginClaim = User.Claims.FirstOrDefault(c => c.Type == Security.JwtTokenOptions.NAME_TYPE);
            Debug.Assert(loginClaim != null);
            return dbContext.Users.First(u => u.Login == loginClaim.Value);
        }

        /// <summary>
        /// Метод аутентификации пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>
        /// Вернет null если пользователь не аутентифицирован
        /// </returns>
        private User AuthenticateUser(string login, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == Security.Hash.GetString(password, System.Text.Encoding.UTF8));
            if (user != default)
                foreach (var role in dbContext.Roles) ;
            return user;
        }

        /// <summary>
        /// Метод создания JWT токена.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="expireDate">Дата истечения токена</param>
        private string GenerateJWT(User user, out DateTime expireDate)
        {
            List<Claim> claims = new List<Claim>() { new Claim(Security.JwtTokenOptions.NAME_TYPE, user.Login) };
            UserToRole[] userToRoles = dbContext.UsersToRoles.Where(u => u.UserId == user.Id).ToArray();
            foreach (var role in userToRoles)
                claims.Add(new Claim("role", dbContext.Roles.First(r => r.Id == role.RoleId).Name));
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims: claims,
                authenticationType: "Token",
                nameType: Security.JwtTokenOptions.NAME_TYPE,
                roleType: Security.JwtTokenOptions.ROLE_TYPE);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Security.AuthOptions.ISSUER,
                audience: Security.AuthOptions.AUDIENCE,
                claims: claimsIdentity.Claims,
                notBefore: DateTime.Now,
                expires: (expireDate = DateTime.Now.Add(Security.AuthOptions.JWT_TOKEN_LIFETIME)),
                signingCredentials: new SigningCredentials(key: Security.AuthOptions.GetSymmetricSecurityKey(), algorithm: SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        /// <summary>
        /// Создать токен обновления.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="token">Основной JWT токен</param>
        /// <param name="refreshTokenExpire">Дата истечения токена</param>
        private string GenerateRefreshToken(User user, string token, out DateTime refreshTokenExpire) 
        {
            refreshTokenExpire = DateTime.Now.Add(Security.AuthOptions.JWT_REFRESH_TOKEN_LIFETIME);
            return Security.Hash.GetString(string.Concat(token.Substring(token.Length - 16, 16), ".", user.Login));
        }

        /// <summary>
        /// Создать IAcionResult создания токена.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenExpire"></param>
        /// <param name="refreshToken"></param>
        /// <param name="refreshTokenExpire"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        private IActionResult CreateJWTActionResult(string token, DateTime tokenExpire, string refreshToken, DateTime refreshTokenExpire, string[] roles)
            => Json(new
            {
                token,
                tokenExpire,
                refreshToken,
                refreshTokenExpire,
                roles
            });
    }
}
