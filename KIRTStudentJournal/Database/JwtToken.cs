using KIRTStudentJournal.Infrastructure;
using Microsoft.VisualBasic.CompilerServices;
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
        /// Заголовок токена
        /// </summary>
        [StringLength(100)]
        public string Header { get; set; }
        /// <summary>
        /// Полезная нагрузка
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(300)]
        public string Payload { get; set; }
        /// <summary>
        /// Подпись
        /// </summary>
        [Key]
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Sign { get; set; }
        /// <summary>
        /// Токен обновления
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(64)]
        public string RefreshToken { get; set; }
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
