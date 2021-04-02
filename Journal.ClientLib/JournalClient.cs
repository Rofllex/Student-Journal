using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Journal.Common.Entities;
using Journal.ClientLib.Entities;
using Journal.ClientLib.Infrastructure;

#nullable enable

namespace Journal.ClientLib
{
    /// <inheritdoc cref="IJournalClient"/>
    public class JournalClient : IJournalClient
    {
        #region public static

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="url"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static JournalClient Connect(string url, string login, string password)
            => throw new NotImplementedException();


        /// <summary>
        ///     Метод подключения к серверу.
        /// </summary>
        /// <param name="url">
        ///     Базовый адрес сервера
        /// </param>
        /// <param name="login">
        ///     Логин
        /// </param>
        /// <param name="password">
        ///     Пароль.
        /// </param>
        /// <exception cref="ExecuteQueryException" />
        /// <exception cref="ConnectFaillureException" />
        public static async Task<JournalClient> ConnectAsync(string url, string login, string password)
        {
            JWTQueryExecuter clientQueryExecuter = new JWTQueryExecuter(new Uri(url), null);
            //JObject? response = await clientQueryExecuter.ExecuteGetQuery<JObject>("Account", "Auth", new string[] { $"login={login}", $"password={password}" }, useToken: false);
            JObject response = await clientQueryExecuter.ExecuteGetQuery<JObject>("Account", "Auth", new Dictionary<string, string>()
            {
                ["login"] = login,
                ["password"] = password
            }, useToken: false);

            if (response != null)
            {
                // Метод проверки и выброса исключения.
                _CheckAuthMethodResponse(response);

                string token = response["token"]?.ToObject<string>() ?? string.Empty,
                        refreshToken = response["refreshToken"]?.ToObject<string>() ?? string.Empty;
                DateTime tokenExpire = response["tokenExpire"]?.ToObject<DateTime>() ?? DateTime.MinValue
                    , refreshTokenExpire = response["refreshTokenExpire"]?.ToObject<DateTime>() ?? DateTime.MinValue;
                clientQueryExecuter.JWTToken = token;
                System.Diagnostics.Debug.Assert(response.ContainsKey("user"));
                User user = response["user"].ToObject<User>();
                if (user != null)
                {
                    JournalClient journalClient = new JournalClient(
                        queryExecuter: clientQueryExecuter
                        , currentUser: user
                        , token: token
                        , tokenExpire: tokenExpire
                        , refreshToken: refreshToken
                        , refreshTokenExpire: refreshTokenExpire);
                    return journalClient;
                }
                else
                    throw new EmptyResponseException();
            }
            else
                throw new EmptyResponseException();
        }

        #endregion

        private JournalClient(IClientQueryExecuter queryExecuter, IUser currentUser, string token, DateTime tokenExpire, string refreshToken, DateTime refreshTokenExpire)
            => (QueryExecuter, User, _token, _tokenExpire, _refreshToken, _refreshTokenExpire) = (queryExecuter, currentUser, token, tokenExpire, refreshToken, refreshTokenExpire);
        
        public IClientQueryExecuter QueryExecuter { get; private set; }

        public IUser User { get; private set; }

        public bool CheckToken()
            => throw new NotImplementedException();
        
        public void RefreshToken()
            => throw new NotImplementedException();
        
        private string _token,
                        _refreshToken;
        private DateTime _tokenExpire,
                        _refreshTokenExpire;
        
        /// <summary>
        ///     Метод проверки ответа от сервера при вызове метода Account/Auth
        /// </summary>
        /// <exception cref="ExecuteQueryException"/>
        private static void _CheckAuthMethodResponse(JObject authMethodResponse)
        {
            string? message = null;

            if (!authMethodResponse.ContainsKey("token"))
                message = "Неверный ответ от метода авторизации. Токен был null.";
            else if (!authMethodResponse.ContainsKey("refreshToken"))
                message = "Неверный ответ от метода авторизации. Токен обновления был null.";
            else if (!authMethodResponse.ContainsKey("tokenExpire"))
                message = "Неверный ответ от метода авторизации. Время истечения токена было null.";
            else if (!authMethodResponse.ContainsKey("refreshTokenExpire"))
                message = "Неверный ответ от метода авторизации. Время истечения токена обновления было null.";
            
            if (message != null)
                throw new ExecuteQueryException(message, authMethodResponse.ToString());
        }
    }
}
