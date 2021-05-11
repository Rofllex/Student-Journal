using Journal.Common.Entities;

using Newtonsoft.Json;

#nullable enable

namespace Journal.ClientLib.Entities
{
    public class Student : IStudent
    {
        [JsonRequired]
        public int UserId { get; set; }
        
        public User? UserEnt { get; set; }

        public int? GroupId { get; set; }

        public StudentGroup GroupEnt { get; set; }

        IStudentGroup? IStudent.Group => GroupEnt;

        IUser? IStudent.User => UserEnt;

        [JsonConstructor]
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        private Student() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    }
}
