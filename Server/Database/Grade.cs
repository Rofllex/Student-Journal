using System;
using System.ComponentModel.DataAnnotations;

using Journal.Common.Entities;

#nullable enable

namespace Journal.Server.Database
{
    /// <summary>
    /// Оценка. 
    /// </summary>
    public class Grade : IGrade
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
        public GradeLevel GradeLevel { get; set; }

        /// <summary>
        /// Объект за который была выставлена оценка
        /// </summary>
        [Required]
        public Subject Subject { get; set; }
        
        /// <summary>
        /// Студент которому была выставлена оценка.
        /// </summary>
        [Required]
        public Student Student { get; set; }

        /// <summary>
        /// Причина выставления оценки
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Преподаватель который выставил оценку.
        /// </summary>
        public Teacher RatedBy { get; set; }

        ITeacher IGrade.RatedBy => RatedBy;

        ISubject IGrade.Subject => Subject;

        IStudent IGrade.Student => Student;

        public Grade()
        {
        }

        public Grade(Teacher ratedBy, Subject subject, Student student, GradeLevel level, DateTime? timestamp = null, string? reason = null)
        {
            RatedBy = ratedBy;
            Subject = subject;
            Student = student;
            GradeLevel = level;
            Timestamp = timestamp.HasValue ? timestamp.Value : DateTime.Now;
            Reason = reason;
        }
    }
}
