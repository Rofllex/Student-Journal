using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public class Role
    {
        public const string ADMIN_ROLE_NAME = "Admin";
        /// <summary>
        /// Роли по умолчанию.
        /// </summary>
        public static IReadOnlyCollection<string> DefaultRoles = new string[] { ADMIN_ROLE_NAME, "Student", "Teacher", "Curator" };

        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        /// <summary>
        /// Название роли
        /// </summary>
        [Required(AllowEmptyStrings = false), MinLength(3)]
        public string Name { get; set; }

        /// <summary>
        /// Связь многие-ко-многим с пользователями.
        /// </summary>
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
