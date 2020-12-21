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

        ISpecialty ISubject.Specialty => Specialty;
    }
}
