using System.ComponentModel.DataAnnotations;



namespace Server.Database
{
    /// <summary>
    /// Связь субъекта с расписанием.
    /// </summary>
    public class TimetableDaySubject
    {
        /// <summary>
        /// Порядковый номер предмета
        /// </summary>
        [Key, Required]
        public byte SubjectIndex { get; set; }
        
        /// <summary>
        /// Предмет.
        /// </summary>
        [Key, Required]
        public Subject Subject { get; set; }
        
        [Key]
        public TimetableDay Day { get; set; }
    }
}
