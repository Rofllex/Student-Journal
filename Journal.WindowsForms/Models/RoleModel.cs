
using Journal.Common.Entities;

namespace Journal.WindowsForms.Models
{
    public class RoleModel : GenericModuleBase<UserRole>
    {
        public RoleModel(UserRole role) : base(role) { }

        public override string ToString()
            => Culture.RoleFormatProvider.Format(Original);
    }
}
