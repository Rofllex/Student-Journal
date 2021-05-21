using System.Collections.Generic;
using System.Threading.Tasks;

using Journal.ClientLib.Entities;

namespace Journal.ClientLib.Infrastructure
{
    public class UsersManager : ControllerManagerBase
    {
        public UsersManager(IJournalClient client)
        {
            base.SetExecuter(client.QueryExecuter);
        }

        private UsersManager() { }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await QueryExecuter.ExecuteGetQuery<Student>(CONTROLLER_NAME, "GetStudent", new Dictionary<string, string>() 
            {
                ["id"] = studentId.ToString()
            });
        }

        public Task<User> GetUserByIdAsync(int userId)
            => QueryExecuter.ExecuteGetQuery<User>(CONTROLLER_NAME, "GetUser", new Dictionary<string, string> 
            {
                ["id"] = userId.ToString()
            });

        private const string CONTROLLER_NAME = "Users";
    }
}
