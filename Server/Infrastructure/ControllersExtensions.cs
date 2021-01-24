using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Journal.Server.Database;

namespace Journal.Server.Infrastructure
{
    public static class ControllersExtensions
    {
        public static User GetUserFromClaims(this Controller controller, JournalDbContext dbContext)
        {
            Claim loginClaim = controller.User.Claims.FirstOrDefault(c => c.Type == Security.JwtTokenOptions.NAME_TYPE);
            if (loginClaim != null)
                return dbContext.Users.FirstOrDefault(u => u.Login == loginClaim.Value);
            else
                return default;
        }

        public static bool UserHasLoginClaim(this Controller controller)
            => controller.User.HasClaim(c => c.Type == Security.JwtTokenOptions.NAME_TYPE);
        
    }
}
