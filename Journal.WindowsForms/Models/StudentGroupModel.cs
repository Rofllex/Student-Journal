using Journal.ClientLib.Entities;

namespace Journal.WindowsForms.Models
{
    public class StudentGroupModel
    {
        public static explicit operator StudentGroup(StudentGroupModel model)
                => model.Original;

        public StudentGroupModel(StudentGroup studentGroup)
        {
            Original = studentGroup;
        }

        public StudentGroup Original { get; private set; }

        public override string ToString()
            => $"{Original.SpecialtyEnt.Name} {Original.CurrentCourse} {Original.Subgroup}";
    }
}
