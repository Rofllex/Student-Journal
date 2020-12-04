using System.Collections.Generic;

#nullable enable
namespace Journal.Common.Entities
{
    /// <summary>
    /// Интерфейс представляющий группу студентов.
    /// </summary>
    public interface IStudentGroup
    {
        /// <summary>
        /// Куратор группы. Может быть <c>null</c> если куратор отсутствует
        /// </summary>
        IUser? Curator { get; }

        /// <summary>
        /// Специальность группы. Не может быть null.
        /// </summary>
        ISpecialty Specialty { get; }
        
        /// <summary>
        /// Текущий курс.
        /// </summary>
        int CurrentCourse { get; }

        /// <summary>
        /// Подгруппа.
        /// </summary>
        int Subgroup { get; }

        /// <summary>
        /// Получить список студентов.
        /// </summary>
        IReadOnlyList<IStudent> GetStudentsList();
    }
}
