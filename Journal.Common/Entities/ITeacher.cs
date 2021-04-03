namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция преподавателя.
    /// </summary>
    public interface ITeacher
    {
        int UserId { get; }
        IUser User { get; }
    }
}
