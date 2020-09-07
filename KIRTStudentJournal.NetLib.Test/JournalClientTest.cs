using System;
using Xunit;

namespace KIRTStudentJournal.NetLib.Test
{
    public class JournalClientTest
    {
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
            Assert.Equal("Admin", journal.Role);
        } 
        [Fact]
        public void FaillureAuth()
        {

        }
        
    }

    public class MockClientBase : ClientBase
    {
        public MockClientBase() : base("")
        {
        
        }

        public static Uri BuildUriTest(Uri baseUri, string method, params string[] args) => ClientBase.BuildUri(baseUri, method, args);

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
