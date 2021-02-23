using Journal.Common.Entities;

using System.Collections.Generic;

#nullable enable

namespace Journal.ClientLib.Entities
{
    public class StudentGroup : IStudentGroup
    {
        public int Id { get; set; }

        public IUser? Curator { get; set; }

        public ISpecialty Specialty { get; set; }

        public int CurrentCourse { get; set; }

        public int Subgroup { get; set; }

        public List<Student> StudentsList { get; set; }

        public IReadOnlyList<IStudent> GetStudentsList()
            => StudentsList;
    }
}
