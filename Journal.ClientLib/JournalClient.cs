using System;
using System.Threading.Tasks;

using Journal.Common.Entities;
using Journal.ClientLib.Entities;
using Newtonsoft.Json.Linq;

#nullable enable

namespace Journal.ClientLib
{
    public class JournalClient
    {
        public IUser CurrentUser { get; private set; }

        /// <summary>
        ///     Панель администратора.
        ///     Может быть null, если пользователь не является администратором.
        /// </summary>
        public AdminPanelClient? AdminPanel { get; private set; }

        
        private JournalClient(JWTJournalClientQueryExecuter queryExecuter, IUser currentUser, string token, DateTime tokenExpire, string refreshToken, DateTime refreshTokenExpire)
        {
            CurrentUser = currentUser;
            _token = token;
            _tokenExpire = tokenExpire;
            _refreshToken = refreshToken;
            _refreshTokenExpire = refreshTokenExpire;
            QueryExecuter = queryExecuter;
        }

        private string _token,
                        _refreshToken;
        private DateTime _tokenExpire,
                        _refreshTokenExpire;
        internal JWTJournalClientQueryExecuter QueryExecuter { get; private set; }

        /// <summary>
        ///     Метод проверки ответа от сервера при вызове метода Account/Auth
        /// </summary>
        /// <exception cref="ExecuteQueryException"/>
        private static void _CheckAuthMethodResponse( JObject authMethodResponse )
        {
            if ( !authMethodResponse.ContainsKey( "token" ) )
                throw new ExecuteQueryException( "Неверный ответ от метода авторизации. Токен был null." );
            else if ( !authMethodResponse.ContainsKey( "refreshToken" ) )
                throw new ExecuteQueryException( "Неверный ответ от метода авторизации. Токен обновления был null." );
            else if ( !authMethodResponse.ContainsKey( "tokenExpire" ) )
                throw new ExecuteQueryException( "Неверный ответ от метода авторизации. Время истечения токена было null." );
            else if ( !authMethodResponse.ContainsKey( "refreshTokenExpire" ) )
                throw new ExecuteQueryException( "Неверный ответ от метода авторизации. Время истечения токена обновления было null." );
        }


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
            JWTJournalClientQueryExecuter clientQueryExecuter = new JWTJournalClientQueryExecuter(new Uri(url));
            JObject? response = await clientQueryExecuter.ExecuteQuery<JObject>("Account", "Auth", new string[] { $"login={login}", $"password={password}" }, useToken: false);
            if ( response != null )
            {
                // Метод проверки и выброса исключения.
                _CheckAuthMethodResponse( response );

                string token = response["token"]?.ToObject<string>() ?? string.Empty,
                        refreshToken = response["refreshToken"]?.ToObject<string>() ?? string.Empty;
                DateTime tokenExpire = response["tokenExpire"]?.ToObject<DateTime>() ?? DateTime.MinValue
                    , refreshTokenExpire = response["refreshTokenExpire"]?.ToObject<DateTime>() ?? DateTime.MinValue;
                clientQueryExecuter.JwtToken = token;
                User? user = await clientQueryExecuter.ExecuteQuery<User>( "Users", "GetMe", Array.Empty<string>() );
                if ( user != null )
                {
                    JournalClient journalClient = new JournalClient(
                        queryExecuter: clientQueryExecuter
                        , currentUser: user
                        , token: token
                        , tokenExpire: tokenExpire
                        , refreshToken: refreshToken
                        , refreshTokenExpire: refreshTokenExpire );
                    return journalClient;
                }
                else
                    throw new EmptyResponseException();
            }
            else
                throw new EmptyResponseException();
        }

#endregion

    }
}
