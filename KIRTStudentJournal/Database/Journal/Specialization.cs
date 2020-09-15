using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIRTStudentJournal.Database.Journal
{
    public class Specialization
    {
        /// <summary>
        /// Id специализации. Можно оставить пустым.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        /// <summary>
        /// Название специализации
        /// </summary>
        /// <example>
        /// Компьютерные системы и комплексы
        /// </example>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        
        /// <summary>
        /// Сокращенное название специализации
        /// </summary>
        /// <example>
        ///     КС
        /// </example>
        [Required(AllowEmptyStrings = false)]
        public string ShortName { get; set; }
    }
}
