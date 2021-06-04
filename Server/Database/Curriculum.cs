using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Journal.Common.Entities;

using Newtonsoft.Json;

namespace Journal.Server.Database
{
    /// <summary>
    ///     Учебный план.
    /// </summary>
    /// <inheritdoc cref="ICurriculum" />
    public class Curriculum : ICurriculum
    {
        [Key
            , DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)
            , MinLength(4)
            , JsonIgnore]
        public string FileName { get; set; }
        
        [Required
            , ForeignKey(nameof(Specialty))]
        public int SpecialtyId { get; set; }

        /// <summary>
        ///     Идентификатор специальности.
        /// </summary>
        public Specialty Specialty { get; set; }

        [Required
            , ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        ISpecialty ICurriculum.Specialty => Specialty;

        ISubject ICurriculum.Subject => Subject;

        public Curriculum() { }

        public Curriculum(Specialty specialty, Subject subject, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));
            this.FileName = fileName;
            
            this.Specialty = specialty ?? throw new ArgumentNullException(nameof(specialty));
            this.Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        }
    }
}
