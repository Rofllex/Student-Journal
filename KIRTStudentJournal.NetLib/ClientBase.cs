using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;
using KIRTStudentJournal.NetLib.Models;
using System.Threading;
using System.Linq;
using KIRTStudentJournal.NetLib.Extensions;
using KIRTStudentJournal.Shared.Models;

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

        protected TokenModel _Token;

        /// <summary>
        ///  Таймаут выполнения запроса
        /// </summary>
        protected TimeSpan ExecuteQueryTimeout { get; set; } = TimeSpan.FromSeconds(1);

        protected ClientBase(string baseUrl) : this (new Uri(baseUrl))
        {
        }
        
        protected ClientBase(Uri baseUri)
        {
            BaseUri = baseUri;
        }

        #region protected

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="ExecuteQueryException"></exception>
        public async Task<HttpResponseMessage> ExecuteQuery(Uri uri)
        {
            HttpResponseMessage message;
            using (HttpClient httpClient = new HttpClient() { Timeout = ExecuteQueryTimeout })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _Token.Token);
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
        public async Task<HttpResponseMessage> ExecuteQueryWithoutToken(Uri uri)
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


        public virtual Uri BuildUri(string relativeMethod, params string[] getArgs) => BuildUri(BaseUri, relativeMethod, getArgs);
        public virtual Uri BuildUri(string relativeMethod, IEnumerable<KeyValuePair<string, string>> getArgs) => BuildUri(BaseUri, relativeMethod, getArgs);
        
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
        protected virtual Uri BuildUri(Uri baseUri, string relativeMethod, params string[] getArgs)
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
        /// Метод создания Uri строки.
        /// Данная реализация неявно вызывает <see cref="BuildUri(Uri, string, string[])"/>
        /// </summary>
        /// <param name="baseUri">Базовый адрес</param>
        /// <param name="relativeMethod">Метод который необходимо вызвать</param>
        /// <param name="getArgs">get аргументы</param>
        /// <returns>Экземпляр класса <see cref="Uri"></see>/></returns>
        protected virtual Uri BuildUri(Uri baseUri, string relativeMethod, IEnumerable<KeyValuePair<string,string>> getArgs)
        {
            using (var enumerator = getArgs.GetEnumerator())
            {
                int argsCount = getArgs.Count();
                string[] args = new string[argsCount];
                if (argsCount > 0)
                    for (int i = 0; enumerator.MoveNext(); i++)
                        args[i] = $"{enumerator.Current.Key}={enumerator.Current.Value}";
                return BuildUri(baseUri, relativeMethod, getArgs);
            }
        }

        #endregion

        /// <summary>
        /// Обновить токен.
        /// </summary>
        public abstract Task RefreshAsync();

        public abstract Task Logout();

        /// <summary>
        /// Освободить ресурсы(если необходимо)
        /// </summary>
        public abstract void Dispose();
    }
}
