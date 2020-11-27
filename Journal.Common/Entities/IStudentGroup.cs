namespace Journal.Common.Entities
{
    public interface IStudentGroup
    {
        IUser Curator { get; }
        ISpecialty Specialty { get; }
        int CurrentCourse { get; }
        int Subgroup { get; }
    }
}
