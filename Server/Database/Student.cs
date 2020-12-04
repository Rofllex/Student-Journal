using Journal.Common.Entities;

namespace Journal.Server.Database
{
    /// <summary>
    /// Класс студента
    /// </summary>
    public class Student : User, IStudent
    {
        /// <summary>
        /// Группа в которой учится студент.
        /// </summary>
        public StudentGroup Group { get; set; }

        IUser IStudent.User => this;

        IStudentGroup IStudent.Group => Group;
    }
}
