using Journal.Common.Entities;

using System.Collections.Generic;

namespace Server.Database
{
    /// <summary>
    /// Класс родителя.
    /// </summary>
    public class Parent : User, IParent
    {
        public List<Student> Childs { get; set; } = new List<Student>();

        IUser IParent.User => this;

        IReadOnlyList<IStudent> IParent.Childs => Childs;
    }
}
