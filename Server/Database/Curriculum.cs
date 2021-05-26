using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace Journal.Server.Database
{
    public class Curriculum
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

        public Specialty Specialty { get; set; }

        [Required
            , ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }


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
