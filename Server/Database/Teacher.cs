using Journal.Common.Entities;

namespace Journal.Server.Database
{
    public class Teacher : User, ITeacher
    {
        IUser ITeacher.User => this;
    }
}
