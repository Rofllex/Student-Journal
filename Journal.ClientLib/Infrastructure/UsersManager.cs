using Journal.ClientLib.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Journal.ClientLib.Infrastructure
{
    public class UsersManager : ControllerManagerBase
    {
        private UsersManager() { }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await QueryExecuter.ExecuteGetQuery<Student>(CONTROLLER_NAME, "GetStudent", new Dictionary<string, string>() 
            {
                ["id"] = studentId.ToString()
            });
        }

        private const string CONTROLLER_NAME = "Users";
    }
}
