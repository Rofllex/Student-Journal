using System.ComponentModel;

using Journal.ClientLib.Entities;

namespace Journal.WindowsForms.Models
{
    public class SpecialtyModel : GenericModuleBase<Specialty>
    {
        public SpecialtyModel(Specialty specialty) : base(specialty)
        {
        }

        [DisplayName("Id")]
        public int Id => Original.Id;

        [DisplayName("Название")]
        public string Name => Original.Name;

        [DisplayName("Код")]
        public string Code => Original.Code;

        [DisplayName("Кол-во курсов")]
        public int MaxCourse => Original.MaxCourse;

        public override string ToString()
            => Name;
    }
}
