using Journal.Common.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace Journal.Server.Database
{
    /// <summary>
    /// Группа студентов.
    /// </summary>
    /// <inheritdoc cref="IStudentGroup"/>
    public class StudentGroup : IStudentGroup
    {
        public StudentGroup(Specialty specialty, int currentCourse, int subgroup, IEnumerable<Student> students, User? curator = null) 
        {
            SpecialtyEnt = specialty ?? throw new ArgumentNullException(nameof(specialty));
            CurrentCourse = currentCourse;
            Subgroup = subgroup;

            if (curator != null)
                CuratorEnt = curator;
            Students = (students != null) ? new List<Student>(students) : new List<Student>();
        }

        // Предупреждение отключено т.к все данные будет записывать entity framework.
#pragma warning disable CS8618
        /// <summary>
        ///     Конструктор для Entity framework`а.
        /// </summary>
        private StudentGroup() { }
#pragma warning restore CS8618

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(CuratorEnt))]
        public int CuratorId { get; set; }

        public User? CuratorEnt { get; set; }

        [ForeignKey(nameof(SpecialtyEnt))]
        public int SpecialtyId { get; set; }

        /// <summary>
        ///     Специальность.
        /// </summary>
        [Required]
        public Specialty SpecialtyEnt { get; set; }
        
       
        public int CurrentCourse { get; set; }

        public int Subgroup { get; set; }

        public List<Student> Students { get; set; }

        public DateTime? GraduatedDate { get; set; }

        #region IStudentGroup implementation

        IUser? IStudentGroup.Curator => CuratorEnt;

        ISpecialty IStudentGroup.Specialty => SpecialtyEnt;

        int IStudentGroup.CurrentCourse => CurrentCourse;

        int IStudentGroup.Subgroup => Subgroup;

        IReadOnlyList<IStudent> IStudentGroup.GetStudentsList() => Students;

        #endregion
    }
}
