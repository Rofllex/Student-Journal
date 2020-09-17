using Newtonsoft.Json;
namespace KIRTStudentJournal.NetLib.Models
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expire_date")]
        public System.DateTime ExpireDate { get; set; }
    }
}
