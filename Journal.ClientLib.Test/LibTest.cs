using System;
using System.Threading.Tasks;

using Xunit;
using Journal.ClientLib;
using Journal.ClientLib.Infrastructure;
using Journal.ClientLib.Entities;

namespace Journal.ClientLib.Test
{
    public class LibTest
    {
        public LibTest() 
        {
            _Init().Wait();
        }

        [Fact]
        public async Task TestAuth()
        {
            JournalClient client =  await JournalClient.ConnectAsync("http://localhost:5000/", "root", "root");
            IControllerManagerFactory factory = new ControllerManagerFactory();
            AdminManager manager = factory.Create<AdminManager>(client);
        }

        [Fact]
        public async Task CreateStudent()
        {

        }

        [Fact]
        public async Task GetSpecialties()
        {
            DatabaseManager dbManager = _managersFactory.Create<DatabaseManager>(_client);
            Specialty[] specialties = await dbManager.GetSpecialtiesAsync(0, 5);
        }

        [Fact]
        public async Task CreateSpecialty()
        { 
            DatabaseManager dbManager = _managersFactory.Create<DatabaseManager>(_client);
            Specialty spec = await dbManager.CreateSpecialtyAsync("Компьютерные системы и комплексы", "0.1.1.1", 4);
            
        }

        [Fact]
        public async Task CreateSubject()
        {
            Subject subj = await _dbManager.CreateSubject("SOME123 SUBJ NAME");
        }

        [Fact]
        public async Task GetSubject()
        {
            Subject[] subjects = await _dbManager.GetSubjects(0, 1);
            Assert.NotEmpty(subjects);
            var subject = subjects[0];
            Assert.NotNull(subject);
            Assert.NotNull(subject.Name);
        }

        private JournalClient _client;
        private IControllerManagerFactory _managersFactory;
        private DatabaseManager _dbManager;

        private async Task _Init()
        {
            _client = await JournalClient.ConnectAsync("http://localhost:5000/", "root", "root");
            _managersFactory = new ControllerManagerFactory();
            _dbManager = _managersFactory.Create<DatabaseManager>(_client);
        }
    }


}
