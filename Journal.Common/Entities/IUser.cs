#nullable enable
namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция представляющая пользователя.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Имя
        /// </summary>
        string FirstName { get; }
        /// <summary>
        /// Фамилия
        /// </summary>
        string Surname { get; }
        /// <summary>
        /// Отчество
        /// </summary>
        string? LastName { get; }
        /// <summary>
        /// Номер телефона.
        /// </summary>
        string? PhoneNumber { get; }
    }
}
