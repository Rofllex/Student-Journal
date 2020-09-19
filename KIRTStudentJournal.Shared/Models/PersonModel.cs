using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KIRTStudentJournal.Shared.Models
{
    public class PersonModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
            
        public string LastName { get; set; }
        [JsonProperty("patronymic")]
        public string Patronymic { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    
        public PersonModel(string firstName, string lastName, string patronymic, string phoneNumber = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            PhoneNumber = phoneNumber;
        }

        public PersonModel()
        {
        }
    }
}
