using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KIRTStudentJournal.Database.Journal;
using KIRTStudentJournal.Shared.Models;
namespace KIRTStudentJournal.Database
{
    public class Account
    {
        /// <summary>
        /// Идентификатор. Генерируется автоматически в базе данных.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Логин аккаунта
        /// </summary>
        [Required(AllowEmptyStrings = false), MinLength(5)]
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
        public Role Role { get; set; }
        
        /// <summary>
        /// Персона к которой привязан аккаунт.
        /// </summary>
        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }

        public virtual int PersonId { get; set; }
        
        public Account(string login, string passwordHash, Role role, Person person)
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
        public bool Compare(Account other) => Login == (other?.Login ?? string.Empty);
    }
}
