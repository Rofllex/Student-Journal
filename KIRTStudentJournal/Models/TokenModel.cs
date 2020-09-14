using KIRTStudentJournal.Database;
using Newtonsoft.Json;
using System;

namespace KIRTStudentJournal.Models
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("expire_date")]
        public DateTime ExpireDate { get; set; }
        public TokenModel(string token, string refreshToken, DateTime expireDate)
        {
            Token = token;
            RefreshToken = refreshToken;
            ExpireDate = expireDate;
        }
        public TokenModel(JwtToken token)
        {
            Token = $"{token.Header}.{token.Payload}.{token.Sign}";
            RefreshToken = token.RefreshToken;
            ExpireDate = token.ExpireDate;
        }
    }
}
