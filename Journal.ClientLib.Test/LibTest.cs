using System;
using System.Threading.Tasks;

using Xunit;
using Journal.ClientLib;

namespace Journal.ClientLib.Test
{
    public class LibTest
    {
        [Fact]
        public async Task Test()
        {
            JournalClient client =  await JournalClient.ConnectAsync("http://localhost:5000/", "root", "root");
        }

        [Fact]
        public async Task CreateStudent()
        {

        }
    }


}
