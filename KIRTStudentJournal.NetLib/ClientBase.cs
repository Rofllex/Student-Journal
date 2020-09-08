using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using KIRTStudentJournal.NetLib.Models;

namespace KIRTStudentJournal.NetLib
{
    /// <summary>
    /// Базовый класс подключения клиента.
    /// </summary>
    public abstract class ClientBase : IDisposable
    {
        /// <summary>
        /// Базовый адрес.
        /// </summary>
        public Uri BaseUri { get; private set; }
        /// <summary>
        /// Токен доступа.
        /// </summary>
        protected string Token { get; set; }
        protected string RefreshToken { get; set; }

        protected TimeSpan ExecuteQueryTimeout { get; set; } = TimeSpan.FromSeconds(1);

        protected ClientBase(string baseUrl) : this (new Uri(baseUrl))
        {
        }
        protected ClientBase(Uri baseUri)
        {
            BaseUri = baseUri;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="ExecuteQueryException"></exception>
        protected async Task<HttpResponseMessage> ExecuteQuery(Uri uri)
        {
            HttpResponseMessage message;
            using (HttpClient httpClient = new HttpClient() { Timeout = ExecuteQueryTimeout })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                    message = await httpClient.GetAsync(uri);
                    return message;
                }
                catch (Exception e) 
                {
                    throw new ExecuteQueryException(e);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="ExecuteQueryException"></exception>
        protected async Task<HttpResponseMessage> ExecuteQueryWithoutToken(Uri uri)
        {
            HttpResponseMessage message;
            using (HttpClient httpClient = new HttpClient() { Timeout = ExecuteQueryTimeout })
            {
                try
                {
                    message = await httpClient.GetAsync(uri);
                    return message;
                }
                catch (Exception e)
                {
                    throw new ExecuteQueryException(e);
                }
            }        
        }
        /// <summary>
        /// Легкий способ построить Uri
        /// </summary>
        /// <param name="baseUri">Базовый адрес. Не может быть null</param>
        /// <param name="relativeMethod">Метод. Не может быть null</param>
        /// <param name="getArgs">GET аргументы</param>
        /// <example>
        /// Пример использования.
        /// <code>    
        /// Uri uri = new Uri("https://localhost:5001/");
        /// BuildUri(uri, "Test/Method", "arg1=1&arg2=2");
        /// </code>
        /// </example>
        protected static Uri BuildUri(Uri baseUri, string relativeMethod, params string[] getArgs)
        {
            UriBuilder builder = new UriBuilder(baseUri) { Path = relativeMethod };
            string getArgsLine;
            IEnumerator enumerator = getArgs.GetEnumerator();
            if (enumerator.MoveNext())
            {
                getArgsLine = "?" + (string)enumerator.Current;
                while (enumerator.MoveNext())
                    getArgsLine += "&" + (string)enumerator.Current;
            }
            else
                getArgsLine = string.Empty;
            builder.Query = getArgsLine;
            return builder.Uri;
        }
        /// <summary>
        /// Обновить токен.
        /// </summary>
        public abstract Task RefreshAsync();

        public virtual void Dispose() 
        {
        }
    }

    public sealed class JournalClient : ClientBase
    {
        public string Role { get; private set; }

        private JournalClient(Uri baseUrl) : base (baseUrl)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ExecuteQueryException"></exception>
        /// <exception cref="RequestErrorException"></exception>
        public override async Task RefreshAsync()
        {
            var response = await ExecuteQuery(BuildUri(BaseUri, "api/Account/Refresh", "refresh_token=" + RefreshToken));
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                JObject jObject = JsonConvert.DeserializeObject<JObject>(responseString);
                if (!Error.IsError(jObject))
                {
                    RefreshToken = jObject["refresh_token"].ToObject<string>();
                    Token = jObject["token"].ToObject<string>();
                }
                else
                {
                    // Выбрасывает исключение но компилятор об этом не знает.
                    jObject.ToObject<Error>().Throw();
                    return;
                }
            }
            else
                throw new ExecuteQueryException(response.StatusCode);
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
            Uri uri = BuildUri(baseUri, "api/Account/SignIn", "login=" + login, "pass=" + password);
            HttpResponseMessage response = await client.ExecuteQueryWithoutToken(uri);
            string contentString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                if (contentString != string.Empty)
                {
                    JObject contentJObject = JsonConvert.DeserializeObject<JObject>(contentString);
                    if (!contentJObject.ContainsKey("error_message"))
                    {
                        string token = contentJObject["token"].ToObject<string>(),
                                role = contentJObject["role"].ToObject<string>(),
                                refresh_token = contentJObject["refresh_token"].ToObject<string>();
                        client.Token = token;
                        client.Role = role;
                        client.RefreshToken = refresh_token;
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
}
