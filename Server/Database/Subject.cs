using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Journal.Common.Entities;

namespace Journal.Server.Database
{
    /// <summary>
    ///     Предмет.
    /// </summary>
    /// <inheritdoc cref="ISubject"/>
    public class Subject : ISubject
    {
        public Subject(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 4)
                throw new ArgumentOutOfRangeException(nameof(name));
            Name = name;
        }

        /// <summary>
        /// Конструктор для EntityFramework`а.
        /// </summary>
        private Subject() { }


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
        
        [Newtonsoft.Json.JsonIgnore]
        public List<Specialty> Specialties { get; set; }
    }
}
