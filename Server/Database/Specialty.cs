using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database
{
    /// <summary>
    /// Специальность
    /// </summary>
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
        public byte MaxCourse { get; set; }

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public Specialty(string name)
        {
            Name = name;
        }

        public Specialty()
        {

        }
    }
}
