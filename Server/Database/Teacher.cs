using Journal.Common.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Journal.Server.Database
{
    public class Teacher : ITeacher
    {
        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Key,
            Required]
        public User User { get; set; }

        [NotMapped]
        IUser ITeacher.User => User;

        public Teacher()
        {

        }

        public Teacher(User user)
        {
            User = user;
        }
    }
}
