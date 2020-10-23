using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Database
{
    /// <summary>
    /// Оценка. 
    /// </summary>
    public class Rating
    {
        public int Id { get; set; }

        /// <summary>
        /// Дата выставления оценки
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }
    
        /// <summary>
        /// Уровень оценки
        /// </summary>
        [Required]
        public RatingLevel Level { get; set; }

        /// <summary>
        /// Объект за который была выставлена оценка
        /// </summary>
        [Required]
        public Subject Subject { get; set; }
        
        /// <summary>
        /// Студент которому была выставлена оценка.
        /// </summary>
        [Required]
        public User User { get; set; }

        public Rating()
        {
        }

        public Rating(Subject subject, User user, RatingLevel level, DateTime? timestamp = null)
        {
            Subject = subject;
            User = user;
            Level = level;
            Timestamp = timestamp.HasValue ? timestamp.Value : DateTime.Now;
        }
    }

    /// <summary>
    /// Оценка. 
    /// </summary>
    public enum RatingLevel
    {
        /// <summary>
        /// НБ
        /// </summary>
        Miss,
        /// <summary>
        /// Зачет
        /// </summary>
        Offset,
        /// <summary>
        /// Незачет
        /// </summary>
        Fail,
        /// <summary>
        /// Два
        /// </summary>
        Two,
        /// <summary>
        /// Три
        /// </summary>
        Three,
        /// <summary>
        /// Четыре
        /// </summary>
        Four,
        /// <summary>
        /// Пять
        /// </summary>
        Five
    }
}
