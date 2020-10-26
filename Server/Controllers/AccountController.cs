using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.IdentityModel.Tokens;

using Server.Database;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Server.Controllers
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
            return Task.Run<IActionResult>(() =>
            {
                var user = AuthenticateUser(login, password);
                if (user != default)
                {
                    return Content(GenerateJWT(user));
                }
                else
                {
                    return Unauthorized();
                }
            });
        }

        public async Task<IActionResult> Register([FromQuery(Name = "login")] string login, [FromQuery(Name="password")] string password)
        {
            return Content("not implemented");
        }

        [Authorize]
        public IActionResult ChangePassword([FromQuery(Name = "newPassword")] string newPassword)
        {
            var user = User;
            return Content("not implemented");
        }

        private User AuthenticateUser(string login, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Login == login && u.PasswordHash == Security.Hash.GetString(password, System.Text.Encoding.UTF8));
            if (user != default)
            {
                foreach (var role in dbContext.Roles)
                {
                }
            }
            return user;
        }

        private string GenerateJWT(User user)
        {
            
            List<Claim> claims = new List<Claim>() { new Claim(Security.TokenOptions.NAME_TYPE, user.Login) };
            UserToRole[] userToRoles = dbContext.UsersToRoles.Where(u => u.UserId == user.Id).ToArray();
            foreach (var role in userToRoles)
                claims.Add(new Claim("role", dbContext.Roles.First(r => r.Id == role.RoleId).Name));
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims: claims,
                authenticationType: "Token",
                nameType: Security.TokenOptions.NAME_TYPE,
                roleType: Security.TokenOptions.ROLE_TYPE);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Security.AuthOptions.ISSUER,
                audience: Security.AuthOptions.AUDIENCE,
                claims: claimsIdentity.Claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.Add(Security.AuthOptions.LIFETIME),
                signingCredentials: new SigningCredentials(key: Security.AuthOptions.GetSymmetricSecurityKey(), algorithm: SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
