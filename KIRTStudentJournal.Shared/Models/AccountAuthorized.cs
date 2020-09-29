using Newtonsoft.Json;

using System.Collections.Generic;
using System.Text;

namespace KIRTStudentJournal.Shared.Models
{
    public class AccountAuthorized
    {   
        /// <summary>
        /// Идентификатор аккаунта
        /// </summary>
        [JsonProperty("id")]
        public uint Id { get; set; }

        /// <summary>
        /// Токен доступа
        /// </summary>
        [JsonProperty("token")]
        public TokenModel Token { get; set; }
        
        /// <summary>
        /// Роль
        /// </summary>
        [JsonProperty("role")]
        public RoleModel Role { get; set; }

        public AccountAuthorized()
        {
        }

        public AccountAuthorized(uint id, TokenModel token, RoleModel role)
        {
            Token = token;
            Role = role;
            Id = id;
        }
    }
}
