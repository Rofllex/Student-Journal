using Journal.ClientLib.Entities;

using System.ComponentModel;

namespace Journal.WindowsForms.Models
{
    public class StudentGroupModel : GenericModuleBase<StudentGroup>
    {
        public static explicit operator StudentGroup(StudentGroupModel model)
                => model.Original;

        public StudentGroupModel(StudentGroup studentGroup) : base(studentGroup) { }

        [DisplayName("ФИО Куратора")]
        public string CuratorName
            => Original.CuratorEnt != null ? $"{Original.CuratorEnt.FirstName} {Original.CuratorEnt.Surname} {Original.CuratorEnt.LastName}" : string.Empty;
                    
        [DisplayName("Название специальности")]
        public string SpecialtyName
            => Original.SpecialtyEnt != null ? Original.SpecialtyEnt.Name : string.Empty;

        [DisplayName("Код специальности")]
        public string SpecialtyCode 
            => Original.SpecialtyEnt.Code;

        [DisplayName("Подгруппа")]
        public int Subgroup
            => Original.Subgroup;


        public override string ToString()
            => $"{Original.SpecialtyEnt.Name} {Original.CurrentCourse} {Original.Subgroup}";
    }
}
