using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Journal.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Journal.Common.Models;

namespace Journal.Server.Infrastructure
{
    public static class ControllersExtensions
    {
        public static User GetUserFromClaims(this Controller controller, DbSet<User> users)
        {
            Claim loginClaim = controller.User.Claims.FirstOrDefault(c => c.Type == Security.JwtTokenOptions.NAME_TYPE);
            if (loginClaim != null)
                return users.FirstOrDefault(u => u.Login == loginClaim.Value);
            else
                return default;
        }

        public static bool UserHasLoginClaim(this Controller controller)
            => controller.User.HasClaim(c => c.Type == Security.JwtTokenOptions.NAME_TYPE);

        public static IActionResult RequestError(this Controller controller, string message, int statusCode)
            => ControllersExtensions.RequestError(controller, new RequestError(message), statusCode);

        public static IActionResult RequestError(this Controller controller, RequestError reqError, int statusCode)
        {
            controller.Response.StatusCode = statusCode;
            return controller.Json(reqError);
        }
    }
}
