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
        public Parent(User user)
        {
            UserEnt = user ?? throw new System.ArgumentNullException(nameof(user));
        }

        private Parent() { }


        [Key
            , ForeignKey(nameof(UserEnt))]
        public int UserId { get; set; }

        public User UserEnt { get; set; }

        public List<Student> ChildStudentEnts { get; set; } = new List<Student>();

        #region IParent impl

        IUser IParent.User => UserEnt;

        IReadOnlyList<IStudent> IParent.ChildStudents => ChildStudentEnts;

        #endregion
    }
}
