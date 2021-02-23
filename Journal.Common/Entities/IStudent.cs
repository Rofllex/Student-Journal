#nullable enable
namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция представляющая студента.
    /// </summary>
    public interface IStudent
    {
        /// <summary>
        ///     Идентификатор пользователя.
        /// </summary>
        int UserId { get; }

        /// <summary>
        ///     Пользователь который является студентом.
        /// </summary>
        IUser User { get; }

        /// <summary>
        ///     Идентификатор группы.
        ///     Может быть null.
        /// </summary>
        int? GroupId { get; }

        /// <summary>
        /// Группа. Может быть null, если студент является абитуриентом.
        /// </summary>
        IStudentGroup? Group { get; } 
    }
}
