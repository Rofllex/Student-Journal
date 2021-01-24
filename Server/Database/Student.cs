using Journal.Common.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace Journal.Server.Database
{
    /* 
     * Проблема использования одинаковых названий и явная реализация членков интерфейса.
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

        
        /// <summary>
        /// Группа в которой учится студент.
        /// Может быть null...
        /// </summary>
        public StudentGroup? GroupEnt { get; set; }


        [NotMapped]
        IStudentGroup? IStudent.Group
            => GroupEnt;
        
        [NotMapped]
        IUser IStudent.User
            => UserEnt;
        

        /// <summary>
        /// Конструктор для Ef.
        /// </summary>
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public Student()
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        {

        }

        public Student(User user, StudentGroup? group = null)
        {
            UserEnt = user;
            GroupEnt = group;
        }
    }
}
