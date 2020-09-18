using Newtonsoft.Json;
using System;

namespace KIRTStudentJournal.Shared.Models
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("expire_date")]
        public DateTime ExpireDate { get; set; }

        public TokenModel()
        {
        }

        public TokenModel(string token, string refreshToken, DateTime expireDate)
        {
            Token = token;
            RefreshToken = refreshToken;
            ExpireDate = expireDate;
        }
    }
}
