using Journal.Common.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace Journal.ClientLib.Infrastructure
{
    public interface IControllerManagerRestriction
    {
        bool Check(IJournalClient client);
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class RoleManagerRestrictionAttribute : Attribute, IControllerManagerRestriction
    {
        public RoleManagerRestrictionAttribute(UserRole requiredRole) 
        {
            _requiredRole = requiredRole;
        }

        public bool Check(IJournalClient client)
            => client.User.Role == _requiredRole;

        private UserRole _requiredRole;
    }
}
