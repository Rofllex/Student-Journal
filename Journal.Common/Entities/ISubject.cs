#nullable enable

namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция предмета.
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        ///     Идентификатор предмета.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Название предмета
        /// </summary>
        string Name { get; }
    }
}
