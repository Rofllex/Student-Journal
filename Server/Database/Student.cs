namespace Server.Database
{
    /// <summary>
    /// Класс студента
    /// </summary>
    public class Student : User
    {
        /// <summary>
        /// Группа в которой учится студент.
        /// </summary>
        public StudentGroup Group { get; set; }
    }
}
