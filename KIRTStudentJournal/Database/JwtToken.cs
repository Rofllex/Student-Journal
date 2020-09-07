using System;
using System.ComponentModel.DataAnnotations;

namespace KIRTStudentJournal.Database
{
    /// <summary>
    /// Сущность для хранения JWT токена в базе данных
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        [Key]
        [StringLength(500)]
        public string Token { get; set; }
        /// <summary>
        /// Аккаунт для кого выдан
        /// </summary>
        [Required]
        public Account GrantedFor { get; set; }
        /// <summary>
        /// Дата истечения.
        /// </summary>
        [Required]
        public DateTime ExpireDate { get; set; }
    }
}
