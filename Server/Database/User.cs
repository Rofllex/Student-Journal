using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database
{
    /// <summary>
    /// Модель пользователя в БД.
    /// </summary>
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [Required(AllowEmptyStrings = false), MinLength(5)]
        public string Login { get; set; }

        /// <summary>
        /// Хэш пароля.
        /// Не может быть null.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата смены пароля.
        /// </summary>
        public DateTime PasswordChanged { get; set; }

        public virtual List<UserToRole> UserRole { get; set; } = new List<UserToRole>(); 

        public User()
        {
        }

        public User(string firstName, string surname, string login, string passwordHash)
        {
            FirstName = firstName;
            Surname = surname;
            Login = login;
            PasswordHash = passwordHash;
            PasswordChanged = DateTime.Now;
        }
    }
}
