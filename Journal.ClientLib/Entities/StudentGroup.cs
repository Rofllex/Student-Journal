using Journal.Common.Entities;

using System;
using System.Collections.Generic;

#nullable enable

namespace Journal.ClientLib.Entities
{
    /// <summary>
    /// Реализация модели группы студентов.
    /// </summary>
    /// <inheritdoc cref="IStudentGroup"/>
    public class StudentGroup : IStudentGroup
    {
        public int Id { get; set; }

        public int CuratorId { get; }
        public IUser? Curator { get; }


        public int SpecialtyId { get; }
        public Specialty SpecialtyEnt { get; }

        public int CurrentCourse { get;  }

        public int Subgroup { get; }

        public List<Student> StudentsList { get; }

        public DateTime? GraduatedDate { get; }

        public IReadOnlyList<IStudent> GetStudentsList()
            => StudentsList;

        ISpecialty IStudentGroup.Specialty { get => SpecialtyEnt; }
    }
}
