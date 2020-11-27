using Microsoft.EntityFrameworkCore;

using MySql.Data.EntityFrameworkCore.Extensions;

using System;

namespace Server.Database
{
    /// <summary>
    /// Контекст для подключения к базе данных.
    /// Прежде чем начать использовать необходимо вызвать метод <see cref="SetConnectionString(string)"/> для установки строки подключения.
    /// Данный экземпляр характеризует только одно подключение к базе данных. 
    /// </summary>
    public class JournalDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        
        public virtual DbSet<Role> Roles { get; set; }
        
        public virtual DbSet<UserToRole> UsersToRoles { get; set; }

        public virtual DbSet<Rating> Ratings { get; set; }

        public virtual DbSet<Specialty> Specialties { get; set; }

        public virtual DbSet<StudentGroup> Groups { get; set; }

        public virtual DbSet<TimetableDay> TimetableDays { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }
        

        private readonly string _connectionString;
        private JournalDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) => optionsBuilder.UseMySQL( _connectionString );
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) => ManyToManyForUserToRole(modelBuilder);    
        

        private void ManyToManyForUserToRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToRole>()
                .HasKey(u => new { u.UserId, u.RoleId });
            modelBuilder.Entity<UserToRole>()
                .HasOne(u => u.User)
                .WithMany(u => u.UserRole)
                .HasForeignKey(k => k.UserId);
            modelBuilder.Entity<UserToRole>()
                .HasOne(r => r.Role)
                .WithMany(r => r.UserToRoles)
                .HasForeignKey(k => k.RoleId);
        }


        #region public static
        
        public static void SetConnectionString(string connectionString) => _createJournalDbContext = () => new JournalDbContext(connectionString); 
        
        private static Func<JournalDbContext> _createJournalDbContext = () => { throw new InvalidOperationException("Подключение к базе данных не настроено"); };
        public static JournalDbContext CreateContext() => _createJournalDbContext();
        
        #endregion
    }
}
