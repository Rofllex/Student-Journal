using System.ComponentModel.DataAnnotations;
using KIRTStudentJournal.Database.Journal;

namespace KIRTStudentJournal.Database
{
    public class Account
    {
        /// <summary>
        /// Логин аккаунта
        /// </summary>
        [Key]
        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Login { get; set; }
        /// <summary>
        /// Хэш пароля
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        [MinLength(64)]
        public string PasswordHash { get; set; }
        /// <summary>
        /// Роль
        /// </summary>
        [Required]
        public Role Role { get; set; }
        
        public virtual Person Person { get; set; }
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
