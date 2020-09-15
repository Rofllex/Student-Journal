using System.Collections.Generic;

namespace KIRTStudentJournal.Database.Journal
{
    /// <summary>
    /// Сущность студента. Наследуюется от <see cref="Person"/>
    /// </summary>
    public class Student : Person
    {
        /// <summary>
        /// Группа студента
        /// </summary>
        public virtual StudentGroup Group { get; set; }

        /// <summary>
        /// Оценки студента.
        /// </summary>
        public virtual List<Enrollment> Enrollments { get; set; }
        public Student(string firstName, string lastName, string patronymic, string phoneNumber, StudentGroup group) : base (firstName, lastName, patronymic, phoneNumber)
        {
            Group = group;
        }

        public Student()
        {
        }
    }
}
