#nullable enable

namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция предмета.
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// Название предмета
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Специальность.
        /// </summary>
        ISpecialty Specialty { get; }
    }
}
