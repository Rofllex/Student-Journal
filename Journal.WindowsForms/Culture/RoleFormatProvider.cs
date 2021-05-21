using Journal.Common.Entities;

namespace Journal.WindowsForms.Culture
{
    public class RoleFormatProvider
    {
        public static string Format(UserRole role)
            => role switch
            {
                UserRole.Admin => "Администратор",
                UserRole.Student => "Студент",
                UserRole.Teacher => "Преподаватель",
                _ => role.ToString()
            };
    }
}
