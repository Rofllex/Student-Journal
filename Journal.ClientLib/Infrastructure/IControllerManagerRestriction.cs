using Journal.Common.Entities;

using System;

namespace Journal.ClientLib.Infrastructure
{
    public interface IControllerManagerRestriction
    {
        bool Check(IJournalClient client);
    }

    /// <summary>
    ///     Атрибут ограничения по ролям.\n Реализует интерфейс <see cref="IControllerManagerRestriction"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class RoleManagerRestrictionAttribute : Attribute, IControllerManagerRestriction
    {
        public RoleManagerRestrictionAttribute(UserRole requiredRole) 
        {
            if (!Enum.IsDefined(typeof(UserRole), requiredRole))
                throw new ArgumentException(nameof(requiredRole));

            _requiredRole = requiredRole;
        }

        /// <summary>
        ///     Проверка роли.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool Check(IJournalClient client)
            => client.User.Role.HasFlag(_requiredRole);

        private UserRole _requiredRole;
    }
}
