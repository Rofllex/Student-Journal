using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Journal.Server.Database;
using System.Linq;
using Journal.Common.Entities;

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
        public void TestGetLinkedEntities()
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

        [Fact]
        public void TestGetChildStudents()
        {
            using (JournalDbContext dbContext = JournalDbContext.CreateContext())
            {
                User parentUser = dbContext.Users.FirstOrDefault(u => u.FirstName == "parent");
                if (parentUser == default)
                {
                    parentUser = new User("parent", "parent", "parent", Security.Hash.GetFromString("hash"), UserRole.StudentParent);
                    Parent parent = new Parent(parentUser);
                    dbContext.Users.Add(parentUser);
                    dbContext.Parents.Add(parent);


                    User studentUser = new User("student", "student", "student", Security.Hash.GetFromString("hash"), UserRole.Student);
                    Student student = new Student(studentUser);
                    dbContext.Users.Add(studentUser);
                    dbContext.Students.Add(student);
                    parent.ChildStudentEnts.Add(student);
                    dbContext.SaveChanges();
                }
                else
                {
                    Parent parent = dbContext.Parents.Include(p => p.ChildStudentEnts).FirstOrDefault(p => p.UserId == parentUser.Id);
                    if (parent != null)
                    {
                        Assert.True(parent.ChildStudentEnts.Count == 1);
                    }
                }
            }
        }
    }
}
