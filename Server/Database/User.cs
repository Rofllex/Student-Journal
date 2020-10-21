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
        /// Логин
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        
        /// <summary>
        /// Хэш пароля
        /// </summary>
        [MaxLength(length: 64)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Связь многие-ко-многим с ролями.
        /// </summary>
        public List<UserToRole> UserRole { get; set; } = new List<UserToRole>();

        public User()
        {
        }

        public User(string firstName, string surname, string lastName, string login, string passwordHash = "")
        {
            FirstName = firstName;
            Surname = surname;
            LastName = lastName;
            Login = login;
            PasswordHash = passwordHash;
        }
    }
}
