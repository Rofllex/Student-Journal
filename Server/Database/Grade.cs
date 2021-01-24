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
        public Subject SubjectEnt { get; set; }
        
        /// <summary>
        /// Студент которому была выставлена оценка.
        /// </summary>
        [Required]
        public Student StudentEnt { get; set; }

        /// <summary>
        /// Причина выставления оценки
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Преподаватель который выставил оценку.
        /// </summary>
        public Teacher RatedByEnt { get; set; }

        ITeacher IGrade.RatedBy => RatedByEnt;

        ISubject IGrade.Subject => SubjectEnt;

        IStudent IGrade.Student => StudentEnt;

        public Grade()
        {
        }

        public Grade(Teacher ratedBy, Subject subject, Student student, GradeLevel level, DateTime? timestamp = null, string? reason = null)
        {
            RatedByEnt = ratedBy;
            SubjectEnt = subject;
            StudentEnt = student;
            GradeLevel = level;
            Timestamp = timestamp.HasValue ? timestamp.Value : DateTime.Now;
            Reason = reason;
        }

    }
}
