using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Journal.Common.Entities;
using Journal.ClientLib.Entities;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Leaf.xNet;

#nullable enable

namespace Journal.ClientLib
{
    internal interface IJournalClientQueryExecuter
    {
        /// <summary>
        ///     Выполнить запрос с передачей агрументов в теле.
        /// </summary>
        /// <typeparam name="T">
        ///     Тип к которому необходимо привести ответ от сервера.
        /// </typeparam>
        /// <param name="controller">
        ///     Контроллер к которому следует обратиться.
        /// </param>
        /// <param name="method">
        ///     Метод к которому следует обратиться
        /// </param>
        /// <param name="args">
        ///     Аргументы запроса.
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации.
        /// </param>
        /// <returns>
        ///     Может вернуть null, если ответ от сервера был пуст.
        /// </returns>
        /// <exception cref="WrongStatusCodeException">
        ///     Если статус код ответа отличается от 200(OK).
        /// </exception>
        /// <exception cref="ConnectFaillureException">
        ///     Если не удалось подключиться к серверу.
        /// </exception>
        Task<T?> ExecutePostQuery<T>( string controller, string method, IDictionary<string, object>? args = null, bool useToken = true ) where T : class;

        /// <summary>
        ///     Выполнить запрос с передачей агрументов в теле.
        /// </summary>
        /// <param name="controller">
        ///     Контроллер к которому следует обратиться.
        /// </param>
        /// <param name="method">
        ///     Метод к которому следует обратиться
        /// </param>
        /// <param name="args">
        ///     Аргументы запроса.
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации.
        /// </param>
        /// <returns>
        ///     Может вернуть null, если ответ от сервера был пуст.
        /// </returns>
        /// <exception cref="WrongStatusCodeException">
        ///     Если статус код ответа отличается от 200(OK).
        /// </exception>
        /// <exception cref="ConnectFaillureException">
        ///     Если не удалось подключиться к серверу.
        /// </exception>
        Task<object?> ExecutePostQuery( string controller, string method, IDictionary<string, object>? args = null, bool useToken = true );

        /// <summary>
        ///     Метод выполнения GET запроса на сервер.
        /// </summary>
        /// <typeparam name="T">Объект к которому необходимо привести ответ.</typeparam>
        /// <param name="controller">
        ///     Контроллер к которому обращаются.
        /// </param>
        /// <param name="method">
        ///     Метод контроллера к которому следует обратиться.
        /// </param>
        /// <param name="args">
        ///     GET аргументы запроса.
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации.
        /// </param>
        /// <returns>
        ///     Может вернуть null, если ответ был пуст.
        /// </returns>
        /// <exception cref="WrongStatusCodeException">Если статус код отличается от 200(OK)</exception>
        /// <exception cref="ConnectFaillureException">
        ///     Если не удалось подключиться к серверу.
        /// </exception>
        Task<T?> ExecuteQuery<T>( string controller, string method, ICollection<string>? args = null, bool useToken = true ) where T : class;

        /// <summary>
        ///     Метод выполнения GET запроса на сервер.
        /// </summary>
        /// <param name="controller">
        ///     Контроллер к которому обращаются.
        /// </param>
        /// <param name="method">
        ///     Метод контроллера к которому следует обратиться.
        /// </param>
        /// <param name="args">
        ///     GET аргументы запроса.
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации.
        /// </param>
        /// <returns>
        ///     Может вернуть null, если ответ был пуст.
        /// </returns>
        /// <exception cref="WrongStatusCodeException">Если статус код отличается от 200(OK)</exception>
        /// <exception cref="ConnectFaillureException">
        ///     Если не удалось подключиться к серверу.
        /// </exception>
        Task<object?> ExecuteQuery( string controller, string method, ICollection<string>? args = null, bool useToken = true );
    }

    internal abstract class JournalClientQueryExecuterBase : IJournalClientQueryExecuter
    {
        public Task<T?> ExecuteQuery<T>( string controller, string method, ICollection<string>? args = null, bool useToken = true ) where T : class
        {
            return Task.Run( () =>
            {
                using ( HttpRequest request = CreateRequest( UriBase, useToken ) )
                {
                    string getArgumentsLine = CreateGetArgumentsLine( args );
                    if ( getArgumentsLine.Length > 0 )
                        getArgumentsLine = getArgumentsLine.Insert( 0, "?" );
                    HttpResponse response;
                    string relativeUrl = $"/api/{controller}/{method}{getArgumentsLine}";
                    try
                    {
                        response = request.Get( relativeUrl );
                    }
                    catch (HttpException ex) when (ex.Status == HttpExceptionStatus.ConnectFailure)
                    {
#if DEBUG
                        throw new ConnectFaillureException( new Uri( request.BaseAddress, relativeUrl ).ToString() );
#else
                        throw new ConnectFaillureException( request.BaseAddress.ToString() );

#endif
                    }

                    if ( response.IsOK )
                    {
                        string responseString = response.ToString();
                        if ( !string.IsNullOrWhiteSpace( responseString ) )
                            return JsonConvert.DeserializeObject<T>( responseString );
                        else
                            return null;
                    }
                    else
                        throw new WrongStatusCodeException( response.StatusCode );
                }
            } );
        }

        public Task<object?> ExecuteQuery( string controller, string method, ICollection<string>? args = null, bool useToken = true )
            => ExecuteQuery<object>( controller, method, args, useToken );

        public Task<T?> ExecutePostQuery<T>( string controller, string method, IDictionary<string, object>? args = null, bool useToken = true ) where T : class
        {
            return Task.Run<T?>( () =>
            {
                using ( HttpRequest request = CreateRequest( UriBase, useToken ) )
                {
                    HttpResponse response;
                    string url = $"/{controller}/{method}";
                    if ( args == null || args.Count == 0 )
                    {
                        response = request.Post( url );
                    }
                    else
                    {
                        StringContent postBody = new StringContent( JsonConvert.SerializeObject( args ) );
                        response = request.Post( url, postBody );
                    }

                    string responseString = response.ToString();
                    if ( response.IsOK )
                        return JsonConvert.DeserializeObject<T>( responseString );
                    else
                        throw new WrongStatusCodeException( response.StatusCode, response: responseString );
                }
            } );
        }

        public Task<object?> ExecutePostQuery( string controller, string method, IDictionary<string, object>? args = null, bool useToken = true )
            => ExecutePostQuery<object>( controller, method, args, useToken );


        protected abstract Uri UriBase { get; }

        protected abstract HttpRequest CreateRequest( Uri uriBase, bool useToken = true );
        
        protected virtual string CreateGetArgumentsLine( ICollection<string>? getArguments )
        {
            string argumentsLine = string.Empty;
            if ( getArguments != null )
            {
                using ( IEnumerator<string> enumerator = getArguments.GetEnumerator() )
                {
                    if ( enumerator.MoveNext() )
                    {
                        argumentsLine = enumerator.Current;
                        while ( enumerator.MoveNext() )
                            argumentsLine += "&" + enumerator.Current;
                        return argumentsLine;
                    }
                    else
                        return argumentsLine;
                }
            }
            else
                return argumentsLine;
        }
    }

    /// <summary>
    ///     Исключение при неудачном подключении.
    /// </summary>
    public class ConnectFaillureException : ExecuteQueryException
    {
        public ConnectFaillureException(string url) : base( $"Исключение при попытке подключения к серверу \"{ url }\"." ) { }
    }

    /// <summary>
    ///     Клиент с передачей токена аутентификации JWT.
    /// </summary>
    internal class JWTJournalClientQueryExecuter : JournalClientQueryExecuterBase
    {
        public string JwtToken { get; set; } = string.Empty;


        public JWTJournalClientQueryExecuter(Uri uriBase)
        {
            _uriBase = uriBase ?? throw new ArgumentNullException(nameof(uriBase));
        }

        protected override Uri UriBase => _uriBase;

        protected override HttpRequest CreateRequest( Uri uriBase, bool useToken )
        {
            HttpRequest request = new HttpRequest( uriBase );
            request.UserAgent = Http.ChromeUserAgent();
            if ( useToken )
                request.AddHeader( "Authorization", "Bearer " + JwtToken );
            return request;
        }

        private Uri _uriBase;
    }

    public class JournalClient
    {
        public IUser CurrentUser { get; set; }

        
        private JournalClient(JWTJournalClientQueryExecuter queryExecuter, IUser currentUser, string token, DateTime tokenExpire, string refreshToken, DateTime refreshTokenExpire)
        {
            CurrentUser = currentUser;
            _token = token;
            _tokenExpire = tokenExpire;
            _refreshToken = refreshToken;
            _refreshTokenExpire = refreshTokenExpire;
            _queryExecuter = queryExecuter;
        }

        private string _token,
                        _refreshToken;
        private DateTime _tokenExpire,
                        _refreshTokenExpire;
        private JWTJournalClientQueryExecuter _queryExecuter;

        /// <summary>
        ///     Not Implemented
        ///     Метод обновления токена.
        /// </summary>
        /// <returns></returns>
        public bool UpdateToken()
        {
            throw new NotImplementedException();

            if ( _refreshTokenExpire <= DateTime.Now )
                return false;
            //_queryExecuter.ExecuteQuery<>
        }

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
