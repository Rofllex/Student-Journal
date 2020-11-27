using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database
{
    /// <summary>
    /// Предмет.
    /// </summary>
    public class Subject
    {
        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)
            , Newtonsoft.Json.JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Название предмета
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Специальностью
        /// </summary>
        [Required]
        public Specialty Specialty { get; set; }
    }
}
