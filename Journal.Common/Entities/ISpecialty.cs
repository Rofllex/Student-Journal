namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция представляющая специальность
    /// </summary>
    public interface ISpecialty
    {
        /// <summary>
        /// Название специальности
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Код специальности
        /// </summary>
        string Code { get; }
        
        /// <summary>
        /// Максимальный курс специальности.
        /// </summary>
        int MaxCourse { get; }
    }
}
