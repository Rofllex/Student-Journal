using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.Xml;

using Microsoft.EntityFrameworkCore;

using Org.BouncyCastle.Asn1.Mozilla;

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

    /// <summary>
    /// Модель пользователя в БД.
    /// </summary>
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Surname { get; set; }
        
        /// <summary>
        /// Отчество
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        
        /// <summary>
        /// Хэш пароля
        /// </summary>
        [MaxLength(length: 64)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Множество ролей.
        /// </summary>
        public List<UserToRole> UserRole { get; set; } = new List<UserToRole>();

        public User()
        {
        }

        public User(string firstName, string surname, string lastName, string login, string passwordHash = "")
        {
            FirstName = firstName;
            Surname = surname;
            LastName = lastName;
            Login = login;
            PasswordHash = passwordHash;
        }
    }

    /// <summary>
    /// Дерьмо случается эта хуйня не поддерживает по умолчанию многие-ко-многим 
    /// </summary>
    public class UserToRole
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    
        public UserToRole()
        {
        }

        public UserToRole(User user, Role role)
        {
            User = user;
            Role = role;
        }
    }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public class Role
    {
        public static IReadOnlyCollection<string> DefaultRoles = new string[] { "Student", "Teacher", "Curator", "SysAdmin" };

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(AllowEmptyStrings = false), MinLength(3)]
        public string Name { get; set; }

        public List<UserToRole> UserToRoles { get; set; } = new List<UserToRole>();

        public Role()
        {
        }

        public Role(string name)
        {
            Name = name;
        }
    }
}
