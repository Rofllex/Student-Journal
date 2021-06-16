using System;

using Microsoft.EntityFrameworkCore;

namespace Journal.Server.Database
{
    /// <summary>
    ///     Контекст для подключения к базе данных.
    ///     Прежде чем начать использовать необходимо вызвать метод <see cref="SetConnectionString(string)"/> для установки строки подключения.
    ///     Данный экземпляр характеризует только одно подключение к базе данных. 
    /// </summary>
    public class JournalDbContext : DbContext
    {
        #region public static
        public static void SetConnectionString(string connectionString)
            => _createJournalDbContext = () => new JournalDbContext(connectionString);

        /// <summary>
        ///     Создание контекста бд.
        ///     Если до вызова этого метода не был вызван метод <see cref="SetConnectionString(string)"/>, то выбросит исключение <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException" />
        public static JournalDbContext CreateContext() => _createJournalDbContext();

        private static Func<JournalDbContext> _createJournalDbContext
            = () => throw new InvalidOperationException("Подключение к базе данных не настроено");
        #endregion

        private readonly string _connectionString;
        public JournalDbContext(string connectionString) => _connectionString = connectionString;


        public DbSet<User> Users { get; set; }
        
        public DbSet<Student> Students { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<StudentGroup> Groups { get; set; }
                
        public DbSet<TimetableDay> TimetableDays { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Parent> Parents { get; set; }

        public DbSet<Grade> Grades { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Curriculum> Curriculums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
