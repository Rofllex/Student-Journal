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

namespace KIRTStudentJournal.Controllers.API
{
    [Controller]
    [Route(template: Infrastructure.API.CONTROLLER_ROUTE)]
    public class UserController : Controller
    {
        public const string ISSUER = "ISSUER";
        public const string AUDIENCE = "AUDIENCE";
        const string KEY = "my_sumer_secret_key";
        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        
        [HttpGet]
        public IActionResult Auth([FromQuery(Name = "login")] string login, [FromQuery(Name = "pass")] string password)
        {
            login = password = "1";
            if (login == "1" && password == "1")
            {
                List<Claim> claims = new List<Claim>() 
                { 
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "1"), 
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "1role"),
                       
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                var jwt = new JwtSecurityToken(
                    issuer: ISSUER,
                    audience: AUDIENCE,
                    notBefore: DateTime.Now,
                    claims: claimsIdentity.Claims,
                    expires: DateTime.Now.Add(TimeSpan.FromHours(1)),
                    signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                dynamic response = new ExpandoObject();
                response.token = encodedJwt;
                
                return Json(response);
            }

            return Json("wrong log or pass");
        }
    }
}
