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
        /// Преподаватель, который выставил оценку.
        /// </summary>
        ITeacher RatedBy { get; }

        /// <summary>
        /// Дата выставления оценки.
        /// </summary>
        DateTime Timestamp { get; }
        
        /// <summary>
        /// Предмет по которому была выставлена оценка.
        /// </summary>
        ISubject Subject { get; }
        
        /// <summary>
        /// За что была выставлена оценка.
        /// </summary>
        string? Reason { get; }

        /// <summary>
        /// Студент, которому была выставлена оценка
        /// </summary>
        IStudent Student { get; }

        /// <summary>
        /// Выставленная оценка.
        /// </summary>
        GradeLevel GradeLevel { get; }
    }
}
