using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIRTStudentJournal.Infrastructure
{
    public static class Jwt
    {
        public const string ISSUER = "ISSUER";
        public const string AUDIENCE = "AUDIENCE";
        const string KEY = "15A550B557D97958187450644713F687C1BEB4A943D54C086BA1DAE16A264030";
        public const int HOURS_LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
