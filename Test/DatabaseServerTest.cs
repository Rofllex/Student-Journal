using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Journal.Server.Database;
using System.Linq;

using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Journal.Server.Test
{
    public class DatabaseServerTest
    {
        public DatabaseServerTest()
        {
            JournalDbContext.SetConnectionString("Server=localhost; Database=studentjournal; Uid=root; Pwd=root; CharSet=utf8");

            using (JournalDbContext dbContext = JournalDbContext.CreateContext())
            {
                if (dbContext.Database.EnsureCreated() || dbContext.Users.FirstOrDefault(u => u.Login == "root") == default)
                {
                    User rootUser = new User()
                    {
                        FirstName = "test",
                        LastName = "test",
                        Login = "root",
                        URole = Common.Entities.UserRole.Admin,
                        PasswordHash = Security.Hash.GetFromString("root"),
                        Surname = "root",
                    };
                    dbContext.Users.Add(rootUser);

                    Student rootStudent = new Student { UserEnt = rootUser };
                    dbContext.Students.Add(rootStudent);
                    dbContext.SaveChanges();
                }
            }
        }
        
        [Fact]
        public void TestGetRoot()
        {
            using (JournalDbContext dbContext = JournalDbContext.CreateContext())
            {
                User user = dbContext.Users.FirstOrDefault(u => u.Login == "root");
                Student student = dbContext.Students.Where(s => s.UserId == user.Id)
                                                    .Include(s => s.UserEnt)
                                                    .FirstOrDefault();
                Assert.NotNull(student);
                Assert.NotNull(student.UserEnt);
            }
        }
    }
}
