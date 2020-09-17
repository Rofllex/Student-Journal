using Newtonsoft.Json;

namespace KIRTStudentJournal.NetLib.Models
{
    public class AccountModel
    {
        [JsonProperty("token")]
        public TokenModel Token { get; set; }

        [JsonProperty("role")]
        public RoleModel Role { get; set; }
    }
}
