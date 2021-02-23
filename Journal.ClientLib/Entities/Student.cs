using Journal.Common.Entities;

#nullable enable

namespace Journal.ClientLib.Entities
{
    public class Student : IStudent
    {
        public int UserId { get; set; }
        
        public IUser User { get; set; }

        public int? GroupId { get; set; }

        public IStudentGroup? Group { get; set; }
    }
}
