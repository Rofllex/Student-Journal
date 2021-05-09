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

        public List<Subject> Subjects { get; set; } = new List<Subject>();

        IReadOnlyList<ISubject> ISpecialty.Subjects => GetSubjectsList();

        public Specialty(string name, string code, int maxCourse, IEnumerable<Subject> subjects)
        {
            Name = name;
            Code = code;
            MaxCourse = maxCourse;
            Subjects.AddRange(subjects);
        }

        public Specialty()
        {

        }

        public IReadOnlyList<ISubject> GetSubjectsList() => Subjects;

        private string _GetDebuggerDisplay() 
            => $"Specialty: {Id} \"{Name}\"";
    }
}
