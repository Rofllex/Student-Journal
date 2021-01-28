using System;

#nullable enable

namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция оценки.
    /// </summary>
    public interface IGrade
    {
        /// <summary>
        /// Идентификатор оценки.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Преподаватель, который выставил оценку.
        /// </summary>
        IUser RatedBy { get; }

        /// <summary>
        /// Дата выставления оценки.
        /// </summary>
        DateTime Timestamp { get; }
        
        /// <summary>
        /// Идентификатор предмета.
        /// </summary>
        int SubjectId { get; }
        
        /// <summary>
        /// Предмет по которому была выставлена оценка.
        /// </summary>
        ISubject? Subject { get; }
        
        /// <summary>
        /// За что была выставлена оценка.
        /// </summary>
        string? Reason { get; }

        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        /// <value>Не может быть 0.</value>
        int StudentId { get; }

        /// <summary>
        /// Студент, которому была выставлена оценка
        /// </summary>
        /// <value>Может быть null./value>
        IStudent? Student { get; }

        /// <summary>
        /// Выставленная оценка.
        /// </summary>
        GradeLevel GradeLevel { get; }
    }
}
