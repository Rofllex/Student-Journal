using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

using Journal.Common.Entities;

namespace Journal.Server.Database
{
    /// <summary>
    /// Специальность
    /// </summary>
    [DebuggerDisplay("{" + nameof(_GetDebuggerDisplay) + "(),nq}")]
    public class Specialty : ISpecialty
    {
        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        [Required(AllowEmptyStrings = false)
            , MinLength(4)]
        public string Name { get; set; }
                    
        /// <summary>
        /// Код специальности
        /// </summary>
        [Required(AllowEmptyStrings = false)
            , MinLength(3)]
        public string Code { get; set; }
        
        /// <summary>
        /// Максимальный курс
        /// </summary>
        [Required]
        public int MaxCourse { get; set; }

        [JsonIgnore]
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public Specialty(string name, string code, int maxCourse)
        {
            Name = name;
            Code = code;
            MaxCourse = maxCourse;
        }

        public Specialty()
        {

        }

        private string _GetDebuggerDisplay() 
            => $"Specialty: {Id} \"{Name}\"";
    }
}
