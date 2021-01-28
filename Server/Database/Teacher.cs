using Journal.Common.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Journal.Server.Database
{
    public class Teacher : ITeacher
    {
        [Key
            , ForeignKey(nameof(UserEnt))]
        public int UserId { get; set; }

        public User UserEnt { get; set; }

        [NotMapped]
        IUser ITeacher.User => UserEnt;

        /// <summary>
        /// Конструктор для EntityFramework`а.
        /// </summary>
        public Teacher()
        {

        }

        public Teacher(User user)
        {
            UserEnt = user;
        }
    }
}
