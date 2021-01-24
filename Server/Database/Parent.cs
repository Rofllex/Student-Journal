using Journal.Common.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Journal.Server.Database
{
    /// <summary>
    /// Класс родителя.
    /// </summary>
    public class Parent : IParent
    {
        [Key
            ,Required]
        public User UserEnt { get; set; }

        public List<Student> ChildEnts { get; set; } = new List<Student>();

        IUser IParent.User => UserEnt;

        IReadOnlyList<IStudent> IParent.Childs => ChildEnts;
    
        public Parent()
        {

        }

        public Parent(User user)
        {
            UserEnt = user;
        }
    }
}
