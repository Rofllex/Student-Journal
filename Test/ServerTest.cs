using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using Xunit;

namespace Test
{
    public class ServerTest
    {
        private const string ADMIN_LOGIN = "admin",
                             ADMIN_PASSWORD = "admin";
        private const string API_BASE = "https://localhost:5001/api";

        [Fact]
        public async void TestRegister()
        {
            using (HttpClient http = new HttpClient())
            {
                await http.GetAsync("https://localhost:5001/api/Account/Auth?login=1&password=1");
            }
        }

        [Fact]
        public async void ChangePassword()
        {
            const string OLD_PASSWORD = "admin",
                         NEW_PASSWORD = "admin1";

            using (HttpClient httpClient = new HttpClient())
            {
                string token = await GetToken(httpClient, ADMIN_LOGIN, ADMIN_PASSWORD);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                string response = await (await httpClient.GetAsync(string.Concat(API_BASE, $"/Account/ChangePassword?oldPassword={OLD_PASSWORD}&newPassword={NEW_PASSWORD}"))).Content.ReadAsStringAsync();
            }
        }


        private async Task<string> GetToken(string login, string password)
        {
            using (HttpClient http = new HttpClient())
                return await GetToken(http, login, password);
        }

        private async Task<string> GetToken(HttpClient httpClient, string login, string password)
        {
            return await (await httpClient.GetAsync($"{API_BASE}/Account/Auth?login={login}&password={password}")).Content.ReadAsStringAsync();
        }
    }
}
