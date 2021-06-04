using Journal.ClientLib.Entities;

namespace Journal.WindowsForms.Models
{
    public class StudentModel : GenericModel<Student>
    {
        public StudentModel(Student student) : base (student) { }

        public override string ToString()
            => $"{Original.UserEnt.FirstName} { Original.UserEnt.Surname }";
    }
}
