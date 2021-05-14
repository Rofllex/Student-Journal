using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Journal.Common.Entities;

using Newtonsoft.Json;

#nullable enable

namespace Journal.Server.Database
{
    /// <summary>
    /// Оценка. 
    /// </summary>
    public class Grade : IGrade
    {
        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Дата выставления оценки
        /// </summary>
        [JsonProperty("timestamp")
            , Required]
        public DateTime Timestamp { get; set; }
    
        /// <summary>
        /// Уровень оценки
        /// </summary>
        [JsonProperty("gradeLevel")
            , Required]
        public GradeLevel GradeLevel { get; set; }

        /// <summary>
        /// Идентификатор предмета.
        /// </summary>
        [Required
            , ForeignKey(nameof(SubjectEnt))]
        public int SubjectId { get; set; }

        /// <summary>
        /// Объект за который была выставлена оценка
        /// </summary>
        public Subject SubjectEnt { get; set; }
        
        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        [Required
            , ForeignKey(nameof(StudentEnt))]
        public int StudentId { get; set; }
        /// <summary>
        /// Студент которому была выставлена оценка.
        /// </summary>
        public Student StudentEnt { get; set; }

        /// <summary>
        /// Причина выставления оценки
        /// </summary>
        /// <value>Может быть null.</value>
        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [Required
            , ForeignKey(nameof(RatedByEnt))]
        public int RatedById { get; set; }

        /// <summary>
        /// Преподаватель который выставил оценку.
        /// </summary>
        public User RatedByEnt { get; set; }

        IUser IGrade.RatedBy => RatedByEnt;

        ISubject IGrade.Subject => SubjectEnt;

        IStudent IGrade.Student => StudentEnt;

        /*
         * Отключение
         */
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        /// <summary>
        ///     Конструктор для EntityFramework`а
        /// </summary>
        public Grade()
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        {
        }

        /// <summary>
        ///     Конструктор выставления оценки.
        /// </summary>
        /// <param name="ratedBy">Кем выставлена оценка</param>
        /// <param name="subject">Предмет по которому выставлена оценка</param>
        /// <param name="student">Студент которому была выставлена оценка</param>
        /// <param name="level">Уровень оценки</param>
        /// <param name="timestamp">Дата выставления. Если значение <c>null</c> то значение будет <see cref="DateTime.Now"/> </param>
        /// <param name="reason">Причина выставления оценки. Может быть null.</param>
        public Grade(User ratedBy, Subject subject, Student student, GradeLevel level, DateTime? timestamp = null, string? reason = null)
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
