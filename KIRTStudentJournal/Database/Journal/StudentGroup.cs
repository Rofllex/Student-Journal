using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KIRTStudentJournal.Database.Journal
{
    /// <summary>
    /// Группа в которой учатся студенты
    /// </summary>
    public class StudentGroup
    {
        /// <summary>
        /// Номер курса
        /// </summary>
        /// <example>
        /// 4
        /// </example>
        [Required(AllowEmptyStrings = false)]
        public byte Course { get; set; }
        /// <summary>
        /// Подгруппа 
        /// </summary>
        /// <example>
        /// 2
        /// </example>
        [Required(AllowEmptyStrings = false)]
        public byte SubGroup { get; set; }
        /// <summary>
        /// Специальность
        /// </summary>
        /// <example>
        /// Компьютерные системы и комплексы
        /// </example>
        [Required(AllowEmptyStrings = false)]
        public string Specialty { get; set; }
        /// <summary>
        /// Короткое название специальности
        /// </summary>
        /// <example>
        /// КС
        /// </example>
        [Required(AllowEmptyStrings = false)]
        public string ShortSpecialty { get; set; }
        
        public List<Person> Students { get; set; }

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
        public StudentGroup(byte course, byte subgroup, string specialty, string shortSpecialty)
        {
            Course = course;
            SubGroup = subgroup;
            Specialty = specialty;
            ShortSpecialty = shortSpecialty;
        }
    }
}
