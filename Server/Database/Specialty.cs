using MySql.Data.EntityFrameworkCore.DataAnnotations;

using Newtonsoft.Json;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Server.Database
{
    /// <summary>
    /// Специальность
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")
        , MySqlCharset("utf8")]
    public class Specialty
    {
        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)
            , JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
                    
        /// <summary>
        /// Код специальности
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }
        
        /// <summary>
        /// Максимальный курс
        /// </summary>
        [Required]
        public int MaxCourse { get; set; }

        [JsonIgnore]
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public Specialty(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public Specialty()
        {

        }

        private string GetDebuggerDisplay() => $"Specialty: {Id} \"{Name}\"";
    }
}
