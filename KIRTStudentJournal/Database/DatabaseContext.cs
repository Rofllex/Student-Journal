using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIRTStudentJournal.Database.Journal;
using Microsoft.EntityFrameworkCore;

namespace KIRTStudentJournal.Database
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<JwtToken> Tokens { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Teacher> Teachers { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual DbSet<Enrollment> Enrollments { get; set; }

        public virtual DbSet<Specialization> Specializations { get; set; }

        public virtual DbSet<StudentGroup> StudentGroups { get; set; }
        public DatabaseContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }


        public static string ConnectionString { private get; set; }
    }
}
