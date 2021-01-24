#nullable enable
using System;

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

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        /// <value>По умолчанию должно быть значение <see cref="UserRole.Guest"/></value>
        UserRole Role { get; }
    }

    /// <summary>
    /// Роль пользователя.
    /// </summary>
    [Flags]
    public enum UserRole
    {
        /// <summary>
        /// Администратор
        /// </summary>
        Admin = 1,
        /// <summary>
        /// Преподаватель
        /// </summary>
        Teacher = 2,
        /// <summary>
        /// Студент
        /// </summary>
        Student = 4,
        /// <summary>
        /// Родитель студента(ов).
        /// </summary>
        StudentParent = 8,
    }
}
