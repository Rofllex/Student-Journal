using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIRTStudentJournal.Database.Journal
{
    public class Enrollment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual Grade Grade { get; set; }
        
        [Required]
        public virtual Student Student { get; set; }
        
        [Required]
        public virtual Subject Subject { get; set; }
        
        [Required]
        public virtual Teacher Teacher { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }

        public string Description { get; set; }

        public Enrollment(Grade grade, Student student, Subject subject, Teacher teacher, DateTime timestamp, string description = null)
        {
            Grade = grade;
            Student = student;
            Subject = subject;
            Teacher = teacher;
            Timestamp = timestamp;
            Description = description ?? string.Empty;
        }

        public Enrollment()
        {
        }
    }
}
