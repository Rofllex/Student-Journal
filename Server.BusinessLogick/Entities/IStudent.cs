using System.Collections.Generic;

namespace Journal.BusinessLogick.Entities
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Имя
        /// </summary>
        string FirstName { get; }
        /// <summary>
        /// Фамилия
        /// </summary>
        string Surname { get; }
        /// <summary>
        /// Отчество
        /// </summary>
        string LastName { get; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        string PhoneNumber { get; }
    }

    /// <summary>
    /// Студент.
    /// </summary>
    public interface IStudent
    {
        IUser User { get; }

        /// <summary>
        /// Группа к которой приписан студент
        /// </summary>
        IStudentGroup InGroup { get; set; }
    }

    public interface ITeacher
    {
        IUser User { get; }
    }

    /// <summary>
    /// Родитель.
    /// </summary>
    public interface IParent
    {
        IUser User { get; }
        ICollection<IStudent> Childs { get; }
    }

    public interface ICurator
    {
        IUser User { get; }
        IStudentGroup Group { get; set; }
    }

    public interface IStudentGroup
    {
        /// <summary>
        /// Студенты которые числятся в группе.
        /// </summary>
        ICollection<IUser> Users { get; }
        
        /// <summary>
        /// Специальность группы.
        /// </summary>
        ISpecialty Specialty { get; }
    }

    /// <summary>
    /// Интерфейс специальности.
    /// </summary>
    public interface ISpecialty
    {
        /// <summary>
        /// Название специальности
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Полное название специальности
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Максимальный курс
        /// </summary>
        int MaxCourse { get; }
    }

    /// <summary>
    /// Предмет.
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// Специальность
        /// </summary>
        ISpecialty Specialty { get; }
    
        /// <summary>
        /// Название предмета
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Полное название предмета.
        /// </summary>
        string FullName { get; }
    }
}
