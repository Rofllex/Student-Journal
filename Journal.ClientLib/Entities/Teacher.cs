using Journal.Common.Entities;

#nullable enable

namespace Journal.ClientLib.Entities
{
    public class Teacher : ITeacher
    {
        [Newtonsoft.Json.JsonConstructor]
        private Teacher() { }

        public int UserId { get; private set; }

        public User UserEnt { get; private set; }

        IUser ITeacher.User => UserEnt;
    }
}
