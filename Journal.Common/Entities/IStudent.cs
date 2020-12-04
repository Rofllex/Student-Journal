#nullable enable
namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция представляющая студента.
    /// </summary>
    public interface IStudent
    {
        /// <summary>
        /// Пользователь который является студентом.
        /// </summary>
        IUser User { get; }

        /// <summary>
        /// Группа. Может быть null, если студент является абитуриентом.
        /// </summary>
        IStudentGroup? Group { get; } 
    }
}
