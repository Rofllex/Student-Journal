using System.ComponentModel.DataAnnotations;

namespace KIRTStudentJournal.Database.Journal
{

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
}
