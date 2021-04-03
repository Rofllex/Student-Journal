using Journal.Common.Entities;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

#nullable enable

namespace Journal.Server.Database
{
    /* 
     * Проблема использования одинаковых названий и явная реализация членов интерфейса.
     * Название UserEnt выбрано из-за того, что если публичный член и явная реализация имеют одинаковые названия членов
     * То EntityFramework будет пытаться загрузить сущность в член который явно реализован IStudent.User.
     * Но у него это не получится и будет выброшено исключение.
     */

    /// <summary>
    /// Класс студента
    /// </summary>
    /// Наследуется от <see cref="Student"/>
    public class Student : IStudent
    {

        /// <summary>
        /// Конструктор для Ef.
        /// </summary>
#pragma warning disable CS8618
        private Student()
#pragma warning restore CS8618 
        {

        }

        public Student(User user, StudentGroup? group = null)
        {
            UserEnt = user ?? throw new System.ArgumentNullException(nameof(user));
            GroupEnt = group;
            if (group != null)
                GroupId = group.Id;
            ParentEnts = new List<Parent>();
        }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        [Key
            ,ForeignKey(nameof(UserEnt))]
        public int UserId { get; set; }

        /// <summary>
        /// Связная сущность пользователя.
        /// Может быть null, если EntityFramework её из бд.
        /// Для того  чтобы работало можно использовать Include(s => s.UserEnt);
        /// </summary>
        public User UserEnt { get; set; }

        [ForeignKey(nameof(GroupEnt))]
        public int? GroupId { get; set; }

        /// <summary>
        ///     Группа в которой учится студент.
        ///     Может быть null.
        /// </summary>
        public StudentGroup? GroupEnt { get; set; }
                
        public List<Parent> ParentEnts { get; set; }

        [NotMapped
            , JsonIgnore]
        IStudentGroup? IStudent.Group
            => GroupEnt;
        
        [NotMapped
            , JsonIgnore]
        IUser IStudent.User
            => UserEnt;
    }
}
