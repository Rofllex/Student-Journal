using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KIRTStudentJournal.NetLib.Models
{
    /// <summary>
    /// Модель роли. Может неявно преобразовываться к <see cref="Role"/>
    /// </summary>
    public class RoleModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("id")]
        public int Id { get; set; }
    
        public RoleModel()
        {
        }

        
        public static implicit operator Role(RoleModel model) => (Role)model.Id;
    }

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
        Teacher
    }
}
