using KIRTStudentJournal.NetLib.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using KIRTStudentJournal.Shared.Models;

namespace KIRTStudentJournal.NetLib.Test
{
    public class JournalClientTest
    {
        private static readonly Uri baseUri = new Uri("https://localhost:5001/");

        [Fact]
        public void BuildUriWithRelativeMethodAndArgs()
        {
            Uri uri = MockClientBase.BuildUriTest(new Uri("http://localhost:5001/"), "Account/SignIn", "login=1", "pass=1");
            Assert.Equal("http://localhost:5001/Account/SignIn?login=1&pass=1", uri.ToString());
        }
        
        [Fact]
        public void BuildUriWithoutArgs()
        {
            Uri uri = MockClientBase.BuildUriTest(new Uri("http://localhost:5001/"), "Account/SignIn");
            Assert.Equal("http://localhost:5001/Account/SignIn", uri.ToString());
        }
        
        [Fact]
        public void BuildUriWithoutMethodAndArgs()
        {
            Uri uri = MockClientBase.BuildUriTest(new Uri("http://localhost:5001/"), "");
            Assert.Equal("http://localhost:5001/", uri.ToString());
        }

        [Fact]
        public async void SuccessAuth()
        {
            var journal = await JournalClient.SignInAsync(new Uri("https://localhost:5001"), "12345", "1");
            Assert.Equal(Role.Admin, journal.Role);
        } 
        
        [Fact]
        public async Task FaillureAuth()
        {
            try
            {
                await JournalClient.SignInAsync(baseUri, "1234576", "1234512");
            }
            catch (Exception e)
            {
                Assert.IsType<RequestErrorException>(e);
            }
        }

        [Fact]
        public async Task ConnectionFail()
        {
            try
            {
                await JournalClient.SignInAsync(new Uri("https://localhost:1021"), string.Empty, string.Empty);
            }
            catch (Exception e)
            {
                Assert.IsType<ExecuteQueryException>(e);
            }
        }
        
    }

    public class PersonModuleTest
    {
        private JournalClient client;
        public PersonModuleTest()
        {
            var signTask = JournalClient.SignInAsync(new Uri("https://localhost:5001"), "12345", "1");
            signTask.Wait();
            client = signTask.Result;
        }

        [Fact]
        public async Task GetMe()
        {
            await client.Person.DEBUG_GetMe();
        }
    }

    public class MockClientBase : ClientBase
    {
        public MockClientBase() : base("")
        {
        
        }

        public static Uri BuildUriTest(Uri baseUri, string method, params string[] args) => new MockClientBase().BuildUri(baseUri, method, args);

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Task Logout()
        {
            throw new NotImplementedException();
        }

        public override Task RefreshAsync()
        {
            throw new NotImplementedException();
        }
    }
}
