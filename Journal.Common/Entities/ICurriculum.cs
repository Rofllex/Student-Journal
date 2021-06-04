
namespace Journal.Common.Entities
{
    /// <summary>
    ///     Абстракция учебного клана.
    /// </summary>
    public interface ICurriculum
    {
        /// <summary>
        ///     Идентификатор.
        /// </summary>
        int Id { get; }

        /// <summary>
        ///     Идентификатор специальности.
        /// </summary>
        int SpecialtyId { get; }

        /// <summary>
        ///     Специальность
        /// </summary>
        ISpecialty Specialty { get; }

        /// <summary>
        ///     Идентификатор предмета.
        /// </summary>
        int SubjectId { get; }

        /// <summary>
        ///     Предмет
        /// </summary>
        ISubject Subject { get; }
    }
}
