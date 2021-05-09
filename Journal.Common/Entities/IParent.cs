using System.Collections.Generic;

namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция представляющая родителя.
    /// </summary>
    public interface IParent
    {
        /// <summary>
        ///     Идентификатор пользователя.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Дети родителя.
        /// </summary>
        IReadOnlyList<IStudent> ChildStudents { get; }
    }
}
