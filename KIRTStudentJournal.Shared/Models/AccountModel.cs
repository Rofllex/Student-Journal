using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace KIRTStudentJournal.Shared.Models
{
    public class AccountModel
    {   
        [JsonProperty("token")]
        public TokenModel Token { get; set; }
        [JsonProperty("role")]
        public RoleModel Role { get; set; }

        public AccountModel()
        {
        }

        public AccountModel(TokenModel token, RoleModel role)
        {
            Token = token;
            Role = role;
        }
    }
}
