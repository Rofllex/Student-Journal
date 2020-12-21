#nullable enable

namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция преподавателя.
    /// </summary>
    public interface ITeacher
    {
        IUser User { get; }
    }
}
