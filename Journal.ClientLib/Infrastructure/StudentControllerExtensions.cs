using Journal.ClientLib.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Journal.ClientLib.Infrastructure
{
    public static class StudentControllerExtensions
    {
        public static Task<Student[]> GetStudentsWithoutGroupAsync(this IJournalClient client, int offset = 0, int count = 100)
            => client.QueryExecuter.ExecuteGetQuery<Student[]>(STUDENTS_CONTROLLER, "GetWithoutGroup", new Dictionary<string, string> 
            {
                ["offset"] = offset.ToString(),
                ["count"] = count.ToString()
            });

        private const string STUDENTS_CONTROLLER = "Students";
    }

    public static class UsersControllerExtensions 
    {
        public static Task<User[]> GetTeachers(this IJournalClient client, int offset = 0, int count = 100)
            => client.QueryExecuter.ExecuteGetQuery<User[]>(USERS_CONTROLLER, "GetTeachers", new Dictionary<string, string> 
            {
                ["offset"] = offset.ToString(),
                ["count"] = count.ToString()
            });
        
        private const string USERS_CONTROLLER = "Users";
    }
}
