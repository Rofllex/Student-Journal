using System;
using Xunit;
using KIRTStudentJournal.Database.Journal;
using KIRTStudentJournal.Database;
using System.Linq;

namespace KIRTStudentJournal.Test
{
    public class DatabaseTest
    {
        public DatabaseTest()
        {
            DatabaseContext.ConnectionString = "server=localhost;UserId=root;Password=root;database=kirtstudentjournal";
        }

        [Fact]
        public void CheckEntities()
        {
            using (var dbContext = new DatabaseContext())
            {
                var account = dbContext.Accounts.Where(a => a.Login == "12345").FirstOrDefault();
                var person = dbContext.Persons.FirstOrDefault(p => p.Id == account.PersonId);
            }   
        }
    }
}
