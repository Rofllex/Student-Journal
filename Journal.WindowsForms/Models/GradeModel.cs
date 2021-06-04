namespace Journal.WindowsForms.Models
{
    public class GradeModel : GenericModel<ClientLib.Entities.Grade>
    {
        public GradeModel(ClientLib.Entities.Grade grade) : base(grade) { }

        public override string ToString()
            => Culture.GradeFormatProvider.ShortFormat(Original.GradeLevel);
    }
}
