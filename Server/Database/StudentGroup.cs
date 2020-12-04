using Journal.Common.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Server.Database
{
    /// <summary>
    /// Группа студентов.
    /// </summary>
    public class StudentGroup : IStudentGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Куратор группы. Может быть null.
        /// </summary>
        public User Curator { get; set; }
        
        /// <summary>
        /// Специальность группы.
        /// </summary>
        [Required]
        public Specialty Specialty { get; set; }

        /// <summary>
        /// Курс 
        /// </summary>
        public int CurrentCourse { get; set; }

        /// <summary>
        /// Подгруппа.
        /// </summary>
        public int Subgroup { get; set; }

        public IReadOnlyList<Student> Students { get; set; } = new List<Student>();

        #region IStudentGroup implementation

        IUser IStudentGroup.Curator => Curator;

        ISpecialty IStudentGroup.Specialty => Specialty;

        int IStudentGroup.CurrentCourse => CurrentCourse;

        int IStudentGroup.Subgroup => Subgroup;

        IReadOnlyList<IStudent> IStudentGroup.GetStudentsList() => Students;

        #endregion
    }
}
