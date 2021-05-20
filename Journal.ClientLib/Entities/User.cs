using Journal.Common.Entities;

using Newtonsoft.Json;

#nullable enable

namespace Journal.ClientLib.Entities
{
    public class User : IUser
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string Surname { get; private set; } = string.Empty;

        public string? LastName { get; private set; }

        public string? PhoneNumber { get; private set; }

        public UserRole Role { get; private set; }

        public User(int id, string firstName, string surname, string lastName, string phoneNumber)
        {
            Id = id;
            FirstName = firstName;
            Surname = surname;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        [JsonConstructor]
        private User() { }

    }
}
