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
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class Specialty
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Название 
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        /// Максимальный курс
        /// </summary>
        [Required]
        public int MaxCourse { get; set; }

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public Specialty(string name)
        {
            Name = name;
        }

        public Specialty()
        {

        }

        private string GetDebuggerDisplay() => $"Specialty: {Id} \"{Name}\"";
    }
}
