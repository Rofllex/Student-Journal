using Journal.Common.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Journal.Server.Database
{
    /// <summary>
    /// Класс родителя.
    /// </summary>
    public class Parent : IParent
    {
        [Key
            , ForeignKey(nameof(UserEnt))]
        public int UserId { get; set; }

        public User UserEnt { get; set; }

        [ForeignKey(nameof(ChildEnts))]
        public List<int> ChildEntIds { get; set; } = new List<int>();
        
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
