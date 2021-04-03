using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

using Journal.Common.Entities;

using Newtonsoft.Json;

namespace Journal.Server.Database
{
    /// <summary>
    /// Модель пользователя в БД.
    /// </summary>
    //[DebuggerDisplay($"User - {nameof(Id)} {nameof(Login)} {nameof(URole)} {nameof(FirstName)} {nameof(Surname)} {nameof(LastName)}")]

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class User : IUser
    {
        private User()
        {
        }

        public User(string firstName, string surname, string login, string passwordHash, UserRole role)
        {
            FirstName = firstName;
            Surname = surname;
            Login = login;
            PasswordHash = passwordHash;
            PasswordChanged = DateTime.Now;
            URole = role;
        }

        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [Required(AllowEmptyStrings = false), 
            MinLength(5), 
            JsonIgnore]
        
        public string Login { get; set; }

        /// <summary>
        /// Хэш пароля.
        /// Не может быть null.
        /// </summary>
        [Required(AllowEmptyStrings = false)
            , JsonIgnore]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Токен обновления.
        /// </summary>
        [JsonIgnore]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Дата смены пароля.
        /// </summary>
        [Required, 
            JsonIgnore]
        public DateTime PasswordChanged { get; set; }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        [Required,
            JsonProperty("Role")]
        public UserRole? URole { get; set; }

        [NotMapped]
        UserRole IUser.Role { get => URole.Value; }

        public bool IsInRole(UserRole role) 
            => URole.HasValue ? URole.Value.HasFlag(role) : false;

        private string GetDebuggerDisplay()
            => $"ID({ Id })  LOGIN: \"{Login}\" ROLE: \"{URole}\"";
    }
}
