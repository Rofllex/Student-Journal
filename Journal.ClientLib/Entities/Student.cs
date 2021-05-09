using Journal.Common.Entities;

#nullable enable

namespace Journal.ClientLib.Entities
{
    public class Student : IStudent
    {
        public int UserId { get; set; }
        
        public IUser User { get; set; }

        public int? GroupId { get; set; }

        public IStudentGroup? Group { get; set; }

        [Newtonsoft.Json.JsonConstructor]
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        private Student() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    }
}
