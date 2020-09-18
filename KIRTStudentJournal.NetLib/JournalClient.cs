using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KIRTStudentJournal.NetLib.Models;
using KIRTStudentJournal.NetLib.Extensions;
using KIRTStudentJournal.Shared.Models;
namespace KIRTStudentJournal.NetLib
{
    public sealed class JournalClient : ClientBase
    {
        /// <summary>
        /// Роль клиента.
        /// </summary>
        public Role Role { get; private set; }

        public PersonClientModule Person { get; private set; }

        private JournalClient(Uri baseUrl) : base (baseUrl)
        {
            Person = new PersonClientModule(this);
        }

        protected override Uri BuildUri(Uri baseUri, string relativeMethod, params string[] getArgs)
        {
            relativeMethod = "api/" + relativeMethod;
            return base.BuildUri(baseUri, relativeMethod, getArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExecuteQueryException"></exception>
        /// <exception cref="RequestErrorException"></exception>
        public override async Task RefreshAsync()
        {
            var response = await ExecuteQuery(BuildUri(BaseUri, "api/Account/Refresh", "refresh_token=" + _Token.RefreshToken));
            if (response.IsSuccessStatusCode)
            {
                var accountModel = await response.Content.ReadAsJsonAndThrowIfError<AccountModel>();
                _Token = accountModel.Token;
                Role = accountModel.Role;
            }
            else
                throw new ExecuteQueryException(response.StatusCode);
        }

        /// <summary>
        /// Выйти из системы.
        /// </summary>
        /// <returns></returns>
        public override async Task Logout()
        {
            var response = await ExecuteQuery(BuildUri(BaseUri, "api/Account/Logout"));
            if (response.IsSuccessStatusCode)
            {
                string str = await response.Content.ReadAsStringAsync();
                if (str.Length > 0)
                {
                    JToken jToken = JsonConvert.DeserializeObject<JToken>(str);
                    if (Error.IsError(jToken))
                        jToken.ToObject<Error>().Throw();
                }
            }
        }

        public override void Dispose()
        {
        }

        /// <summary>
        /// Асинхронный метод авторизации на сервере
        /// </summary>
        /// <param name="baseUri">Базовый адрес</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        /// <exception cref="RequestErrorException"></exception>
        /// <exception cref="ExecuteQueryException"></exception>
        /// <exception cref="Exception"></exception>
        public static async Task<JournalClient> SignInAsync(Uri baseUri, string login, string password)
        {
            JournalClient client = new JournalClient(baseUri);
            Uri uri = client.BuildUri(baseUri, "Account/SignIn", "login=" + login, "pass=" + password);
            HttpResponseMessage response = await client.ExecuteQueryWithoutToken(uri);
            string contentString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                if (contentString != string.Empty)
                {
                    JObject contentJObject = JsonConvert.DeserializeObject<JObject>(contentString);
                    if (!Error.IsError(contentJObject))
                    {
                        client._Token = contentJObject["token"].ToObject<TokenModel>();
                        client.Role = contentJObject["role"].ToObject<RoleModel>();
                        return client;
                    }
                    else
                        throw new RequestErrorException(contentJObject.ToObject<Error>());
                }
                else
                    throw new Exception("Ответ от сервера был пустым");
            }
            else
                throw new ExecuteQueryException(response.StatusCode);
        }
    }

    public class PersonClientModule
    {
        private JournalClient client;
        public PersonClientModule(JournalClient client)
        {
            this.client = client;
        }

        public async Task DEBUG_GetMe()
        {
            var uri = client.BuildUri("Person/GetMe");
            var resposne = await client.ExecuteQuery(uri);
            
        }
    }
}
