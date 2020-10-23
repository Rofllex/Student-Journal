using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace Server.Database
{
    public class Timetable
    {
        [Required]
        public User ApprovedBy { get; set; }
        [Required]
        public StudentGroup Group { get; set; }
        [Required]
        public DateTime Date { get; set; }

    }

    public class TimetableDay
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public User ApprovedBy { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public ICollection<TimetableDaySubject> Subjects { get; set; }
    }

    public class TimetableDaySubject
    {
        [Key, Required]
        public byte SubjectIndex { get; set; }
        [Key, Required]
        public Subject Subject { get; set; }
        [Key]
        public TimetableDay Day { get; set; }
    }
}
