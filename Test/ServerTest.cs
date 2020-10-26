using System;
using System.Net;
using System.Net.Http;

using Xunit;

namespace Test
{
    public class ServerTest
    {
        [Fact]
        public async void TestRegister()
        {
            using (HttpClient http = new HttpClient())
            {
                await http.GetAsync("https://localhost:5001/api/Account/Auth?login=1&password=1");
            }
        }
    }
}
