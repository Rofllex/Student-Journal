using System.ComponentModel;

using Journal.ClientLib.Entities;

namespace Journal.WindowsForms.Models
{
    public class SpecialtyModel
    {
        public SpecialtyModel(Specialty specialty)
        {
            _specialty = specialty;
        }

        [Browsable(false)]
        public Specialty Original => _specialty;

        [DisplayName("Id")]
        public int Id => _specialty.Id;

        [DisplayName("Название")]
        public string Name => _specialty.Name;

        [DisplayName("Код")]
        public string Code => _specialty.Code;

        [DisplayName("Кол-во курсов")]
        public int MaxCourse => _specialty.MaxCourse;

        private Specialty _specialty;
    }
}
