using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database
{
    /// <summary>
    /// Группа студентов.
    /// </summary>
    public class StudentGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Куратор группы. Может быть null.
        /// </summary>
        public User Curator { get; set; }
        
        /// <summary>
        /// Специальность группы.
        /// </summary>
        [Required]
        public Specialty Specialty { get; set; }

        /// <summary>
        /// Курс 
        /// </summary>
        public int CurrentCource { get; set; }

        /// <summary>
        /// Подгруппа.
        /// </summary>
        public int Subgroup { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        

    }
}
