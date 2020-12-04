using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Journal.Server.Security
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
        public static readonly TimeSpan JWT_TOKEN_LIFETIME = new TimeSpan(hours: 0, minutes: 15, seconds: 0);

        /// <summary>
        /// Время жизни токена обновления.
        /// </summary>
        public static readonly TimeSpan JWT_REFRESH_TOKEN_LIFETIME = new TimeSpan(hours: 24, minutes: 0, seconds: 0);

        private const string KEY = "8ih0w03dk0s5in697mpw5ombjt92z9ho";
        /// <summary>
        /// Получить симметричный ключ безопасности.
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
