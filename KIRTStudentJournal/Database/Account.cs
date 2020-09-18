using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KIRTStudentJournal.Database.Journal;

namespace KIRTStudentJournal.Database
{
    public class Account
    {
        /// <summary>
        /// Логин аккаунта
        /// </summary>
        [Key, Required(AllowEmptyStrings = false), MinLength(5)]
        public string Login { get; set; }
        
        /// <summary>
        /// Хэш пароля
        /// </summary>
        [Required(AllowEmptyStrings = false), MinLength(64), MaxLength(64)]
        public string PasswordHash { get; set; }
        
        /// <summary>
        /// Роль
        /// </summary>
        [Required]
        public Shared.Models.Role Role { get; set; }
        
        /// <summary>
        /// Персона к которой привязан аккаунт.
        /// </summary>
        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }

        public virtual int PersonId { get; set; }
        
        public Account(string login, string passwordHash, Shared.Models.Role role, Person person)
        {
            Login = login;
            PasswordHash = passwordHash;
            Role = role;
            Person = person;
        }

        public Account()
        {
        }
        
        /// <summary>
        /// Метод сравнения аккаунта.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Compare(Account other) => other != null
                                                && Login == other.Login
                                                && PasswordHash == other.PasswordHash;
    }
}
