using KIRTStudentJournal.Database;
using Newtonsoft.Json;

namespace KIRTStudentJournal.Models
{
    public class AccountModel
    {
        /// <summary>
        /// Токен доступа
        /// Может быть null.
        /// </summary>
        [JsonProperty("token")]
        public TokenModel Token { get; set; }
        /// <summary>
        /// Роль.
        /// </summary>
        [JsonProperty("role")]
        public RoleModel Role { get; set; }

        public AccountModel(Account account, JwtToken token) : this (new RoleModel(account.Role), new TokenModel(token))
        {
        }

        public AccountModel(RoleModel roleModel, TokenModel token)
        {
            Token = token;
            Role = roleModel;
        }
    }
}
