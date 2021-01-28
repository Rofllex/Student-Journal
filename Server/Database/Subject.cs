using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Journal.Common.Entities;

namespace Journal.Server.Database
{
    /// <summary>
    /// Предмет.
    /// </summary>
    public class Subject : ISubject
    {
        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Название предмета.
        /// </summary>
        /// <value>Минимальная длина 4 символа.</value>
        [Required(AllowEmptyStrings = false)
            , MinLength(4)]
        public string Name { get; set; }

        /// <summary>
        /// Специальность. 
        /// </summary>
        [Required]
        public Specialty SpecialtyEnt { get; set; }

        ISpecialty ISubject.Specialty => SpecialtyEnt;
    
        /// <summary>
        /// Конструктор для EntityFramework`а.
        /// </summary>
        public Subject() { }

        public Subject(string name, Specialty specialty)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 4)
                throw new ArgumentOutOfRangeException(nameof(name));
            Name = name;
            SpecialtyEnt = specialty ?? throw new System.ArgumentNullException(nameof(specialty));
        }
    }
}
