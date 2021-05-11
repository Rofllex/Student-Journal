using Journal.ClientLib.Entities;

namespace Journal.WindowsForms.Models
{
    public class StudentModel : GenericModuleBase<Student>
    {
        public StudentModel(Student student) : base (student) { }

        public override string ToString()
            => $"{Original.UserEnt.FirstName} { Original.UserEnt.Surname }";
    }
}
