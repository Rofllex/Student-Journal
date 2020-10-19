using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Server.Database
{
    public class JournalDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        
        public JournalDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseMySQL(_connectionString);
        

        private static string _connectionString = string.Empty;
        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }
    }

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

        [Required]
        public virtual UserRole Role { get; set; }
    
        public User()
        {
        }

        public User(string firstName, string surname, string lastName, string login, string passwordHash = "", UserRole role = UserRole.Student)
        {
            FirstName = firstName;
            Surname = surname;
            LastName = lastName;
            Login = login;
            PasswordHash = passwordHash;
            Role = role;
        }
    }

    [Flags]
    public enum UserRole
    {
        /// <summary>
        /// Студент
        /// </summary>
        Student = 1,
        /// <summary>
        /// Преподаватель
        /// </summary>
        Teacher = 2,
        /// <summary>
        /// Куратор группы.
        /// </summary>
        Curator = 4,
        /// <summary>
        /// Администратор системы
        /// </summary>
        SysAdmin = 8,
    }
}
