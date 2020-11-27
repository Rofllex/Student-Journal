namespace Journal.Common.Entities
{
    public interface ISpecialty
    {
        string Name { get; }
        string Code { get; }
        int MaxCourse { get; }
    }
}
