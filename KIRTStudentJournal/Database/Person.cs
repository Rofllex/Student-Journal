using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace KIRTStudentJournal.Database
{
    /// <summary>
    /// Роли
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Администратор
        /// </summary>
        Admin,
        /// <summary>
        /// Студент
        /// </summary>
        Student,
        /// <summary>
        /// Родитель студента
        /// </summary>
        StudentParent,
        /// <summary>
        /// Преподаватель
        /// </summary>
        Teacher,
    }

    /// <summary>
    /// Модель представляющая личность
    /// </summary>
    public class Person
    {
        [Key]
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
        public string LastName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }

    public class Account
    {
        /// <summary>
        /// Логин аккаунта
        /// </summary>
        [Key]
        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Login { get; set; }
        /// <summary>
        /// Хэш аккаунта
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        [MinLength(64)]
        public string PasswordHash { get; set; }
        [Required]
        public Role Role { get; set; }
        public Person Person { get; set; }

        public bool Compare(Account other) => other != null
                                                && Login == other.Login 
                                                && PasswordHash == other.PasswordHash;
        
    }
}
