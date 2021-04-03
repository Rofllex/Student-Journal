using Journal.Common.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Journal.Server.Database
{
    public class Teacher : ITeacher
    {
        /// <summary>
        /// Конструктор для EntityFramework`а.
        /// </summary>
        private Teacher()
        {

        }

        public Teacher(User user)
        {
            UserEnt = user ?? throw new System.ArgumentNullException(nameof(user));
        }

        [Key
            , ForeignKey(nameof(UserEnt))]
        public int UserId { get; set; }

        [Required]
        public User UserEnt { get; set; }

        [NotMapped]
        IUser ITeacher.User => UserEnt;
    }
}
