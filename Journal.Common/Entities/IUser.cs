#nullable enable
using System;

using Newtonsoft.Json;

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
        [JsonProperty("id")]
        int Id { get; }

        /// <summary>
        /// Имя
        /// </summary>
        [JsonProperty("firstName")]
        string FirstName { get; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [JsonProperty("surname")]
        string Surname { get; }

        /// <summary>
        /// Отчество
        /// </summary>
        [JsonProperty("lastName")]
        string? LastName { get; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        [JsonProperty("phoneNumber")]
        string? PhoneNumber { get; }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        /// <value>По умолчанию должно быть значение <see cref="UserRole.Guest"/></value>
        [JsonProperty("role")]
        UserRole Role { get; }
    }

    /// <summary>
    /// Роль пользователя.
    /// </summary>
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
