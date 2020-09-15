using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KIRTStudentJournal.Database.Journal
{
    /// <summary>
    /// Сущность субъекта.
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Идентификатор предмета.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Название предмета
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        
        /// <summary>
        /// Сокращенное название предмета. Может быть идентично Name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string ShortName { get; set; }

        public Subject(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }

        public Subject()
        {
        }
    }
}
