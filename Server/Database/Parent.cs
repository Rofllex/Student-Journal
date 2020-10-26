using System.Collections.Generic;

namespace Server.Database
{
    /// <summary>
    /// Класс родителя.
    /// </summary>
    public class Parent : User
    {
        public List<Student> Childs { get; set; } = new List<Student>();
    }
}
