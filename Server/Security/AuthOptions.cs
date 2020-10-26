using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Security
{
    public static class AuthOptions
    {
        /// <summary>
        /// Издатель токена
        /// </summary>
        public static string ISSUER => "StudentJournal-yQdfWlvYso";

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public static string AUDIENCE => "StudentJournalClient-JoocuCa12Z";
        
        /// <summary>
        /// Время жизни токена
        /// </summary>
        public static readonly TimeSpan LIFETIME = new TimeSpan(1, 0, 0);


        private const string KEY = "8ih0w03dk0s5in697mpw5ombjt92z9ho";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
