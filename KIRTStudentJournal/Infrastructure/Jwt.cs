using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace KIRTStudentJournal.Infrastructure
{
    public static class Jwt
    {
        public const string ISSUER = "ISSUER";
        public const string AUDIENCE = "AUDIENCE";
        const string KEY = "15A550B557D97958187450644713F687C1BEB4A943D54C086BA1DAE16A264030";
        public const int HOURS_LIFETIME = 1;
        public const string DEFAULT_LOGIN_TYPE = "login";
        public const string DEFAULT_ROLE_TYPE = "type";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }

    public class ParsedJwtToken 
    {
        public string Header { get; private set; }
        public string Payload { get; private set; }
        public string Sign { get; private set; }

        public ParsedJwtToken(string fullJwtToken)
        {
            string[] splitted = fullJwtToken.Split('.');
            Header = splitted[0];
            Payload = splitted[1];
            Sign = splitted[2];
        }

        public static bool TryParse(string fullJwtToken, out ParsedJwtToken parsedToken)
        {
            if (fullJwtToken.Count(c => c == '.') == 3)
            {
                parsedToken = new ParsedJwtToken(fullJwtToken);
                return true;
            }
            parsedToken = default;
            return false;
        }
    }

}
