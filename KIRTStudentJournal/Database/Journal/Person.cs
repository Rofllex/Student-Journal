using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIRTStudentJournal.Database.Journal
{

    /// <summary>
    /// Модель представляющая личность
    /// </summary>
    public class Person
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        
        [Key, Required(AllowEmptyStrings = false)]
        public virtual Account Account { get; set; }
        
        public Person()
        {
        }

        public Person(string firstName, string lastName, string patronymic, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            PhoneNumber = phoneNumber;
        }
    }
}
