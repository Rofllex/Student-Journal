using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIRTStudentJournal.Database.Journal
{
    /// <summary>
    /// Группа в которой учатся студенты
    /// </summary>
    public class StudentGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Номер курса
        /// </summary>
        /// <example>
        /// 4
        /// </example>
        [Required]
        public byte Course { get; set; }
     
        /// <summary>
        /// Подгруппа 
        /// </summary>
        /// <example>
        /// 2
        /// </example>
        [Required]
        public byte SubGroup { get; set; }

        /// <summary>
        /// Специализация.
        /// </summary>
        [Required]
        public virtual Specialization Specialization { get; set; }

        /// <summary>
        /// Куратор группы. Может быть null.
        /// </summary>
        public virtual Person Curator { get; set; }

        public virtual List<Person> Students { get; set; }

        public StudentGroup()
        {

        }

        /// <summary>
        /// Конструктор <see cref="StudentGroup"/>
        /// </summary>
        /// <param name="course">Курс</param>
        /// <param name="subgroup">Номер подгруппы</param>
        /// <param name="specialty">Специальность</param>
        /// <param name="shortSpecialty">Короткое название специальнеости</param>
        public StudentGroup(byte course, byte subgroup,  Specialization specialization)
        {
            Course = course;
            SubGroup = subgroup;
            Specialization = specialization;
        }
    }
}
