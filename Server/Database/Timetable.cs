using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Server.Database
{
    /// <summary>
    /// Дань расписания.
    /// </summary>
    public class TimetableDay
    {
        /// <summary>
        /// Идентификатор дня.ы
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Кем утверждено.
        /// </summary>
        [Required]
        public User ApprovedBy { get; set; }
        
        /// <summary>
        /// Расписание на день.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Предметы.
        /// </summary>
        public ICollection<TimetableDaySubject> Subjects { get; set; }


        /// <summary>
        /// Если расписание было изменено
        /// </summary>
        public bool IsChanged { get; set; } = false;

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime? ChangeDate { get; set; } = DateTime.MinValue;
    }
}
