using Journal.Common.Entities;

using System.Collections.Generic;

#nullable enable

namespace Journal.ClientLib.Entities
{
    public class StudentGroup : IStudentGroup
    {
        public int Id { get; set; }

        public IUser? Curator { get; }

        public ISpecialty Specialty { get; }

        public int CurrentCourse { get;  }

        public int Subgroup { get; }

        public List<Student> StudentsList { get; }

        public IReadOnlyList<IStudent> GetStudentsList()
            => StudentsList;
    }

    public class Specialty : ISpecialty
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int MaxCourse { get; set; }
    }
}
