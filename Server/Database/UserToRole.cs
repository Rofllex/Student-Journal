namespace Server.Database
{
    /// <summary>
    /// Дерьмо случается эта хуйня не поддерживает по умолчанию многие-ко-многим 
    /// </summary>
    public class UserToRole
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    
        public UserToRole()
        {
        }

        public UserToRole(User user, Role role)
        {
            User = user;
            Role = role;
        }
    }
}
