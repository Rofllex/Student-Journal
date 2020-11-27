namespace Journal.Common.Entities
{
    public interface IUser
    {
        string FirstName { get; }
        string Surname { get; }
        string LastName { get; }
        string PhoneNumber { get; }
    }
}
