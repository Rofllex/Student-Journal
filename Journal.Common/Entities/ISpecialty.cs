namespace Journal.Common.Entities
{
    /// <summary>
    /// Абстракция представляющая специальность
    /// </summary>
    public interface ISpecialty
    {
        public int Id { get; }

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
