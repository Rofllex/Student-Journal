using Microsoft.EntityFrameworkCore;

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

        public JournalDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseMySQL(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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


        private static string _connectionString = string.Empty;
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
