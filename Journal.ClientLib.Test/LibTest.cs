using System;
using System.Threading.Tasks;

using Xunit;
using Journal.ClientLib;
using Journal.ClientLib.Infrastructure;

namespace Journal.ClientLib.Test
{
    public class LibTest
    {
        [Fact]
        public async Task TestAuth()
        {
            JournalClient client =  await JournalClient.ConnectAsync("http://localhost:5000/", "root", "root");
            IControllerManagerFactory factory = new ControllerManagerFactory();
            AdminPanelManager manager = factory.Create<AdminPanelManager>(client);
        }

        [Fact]
        public async Task CreateStudent()
        {

        }
    }


}
