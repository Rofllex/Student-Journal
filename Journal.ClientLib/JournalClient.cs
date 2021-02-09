using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Journal.Common.Entities;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Leaf.xNet;

#nullable enable

namespace Journal.ClientLib
{
    internal interface IJournalClientQueryExecuter
    {
        Task<T> ExecuteQuery<T>(string controller, string method, ICollection<string> args);
        Task<object> ExecuteQuery(string controller, string method, ICollection<string> args);
    }

    internal class JournalClientQueryExecuter : IJournalClientQueryExecuter
    {
        public string JwtToken
        {
            get 
            {
                if (!string.IsNullOrWhiteSpace(_request.Authorization))
                {
                    string[] splittedJwt = _request.Authorization.Split(" ");
                    if (splittedJwt.Length == 2)
                        return splittedJwt[1];
                    else
                        return null;
                }
                else
                    return null;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _request.Authorization = string.Empty;
                }
                else
                {
                    _request.Authorization = "Bearer " + value;
                }
            }
        }

        public JournalClientQueryExecuter(Uri uriBase)
        {
            _uriBase = uriBase ?? throw new ArgumentNullException(nameof(uriBase));
        }

        public Task<T> ExecuteQuery<T>(string controller, string method, ICollection<string>? args = null)
        {
            return Task.Run(() =>
            {
                // TODO: может выбросить исключение если нет подключения к серверу.
                HttpResponse response = _request.Get(_CreateUri(_uriBase, controller, method, args));
                if (response.IsOK)
                {
                    string responseString = response.ToString();
                    if (!string.IsNullOrWhiteSpace(responseString))
                        return JsonConvert.DeserializeObject<T>(responseString);
                    else
                        throw new EmptyResponseException();
                }
                else
                    throw new WrongStatusCodeException(response.StatusCode);

            });
        }

        public Task<object> ExecuteQuery(string controller, string method, ICollection<string> args) 
            => ExecuteQuery<object>(controller, method, args);


        private Uri _uriBase;
        private HttpRequest _request = new HttpRequest();

        private Uri _CreateUri(Uri uriBase, string controller, string method, ICollection<string>? args)
        {
            string argumentsLine = _CreateArgumentsLine(args);
            if (argumentsLine.Length > 0)
                argumentsLine = $"?{argumentsLine}";
            Uri uri = new Uri(uriBase, $"api/{controller}/{method}{argumentsLine}");
            return uri;
        }

        private string _CreateArgumentsLine(ICollection<string>? args)
        {
            if (args != null)
            {
                IEnumerator<string> enumerator = args.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    string argsLine = (string)enumerator.Current;
                    while (enumerator.MoveNext())
                        argsLine += "&" + (string)enumerator.Current;
                    return argsLine;
                }
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }
    }

    public class JournalClient
    {
        public IUser CurrentUser { get; set; }

        
        private JournalClient(IUser currentUser, string token, DateTime tokenExpire, string refreshToken, DateTime refreshTokenExpire) 
        {
            _token = token;
            _tokenExpire = tokenExpire;
            _refreshToken = refreshToken;
            _refreshTokenExpire = refreshTokenExpire;
        }

        private string _token,
                        _refreshToken;
        private DateTime _tokenExpire,
                        _refreshTokenExpire;
        private JournalClientQueryExecuter _queryExecuter;

        public static JournalClient Connect(string url, string login, string password)
        {
            throw new NotImplementedException();    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ExecuteQueryException" />
        public static async Task<JournalClient> ConnectAsync(string url, string login, string password)
        {
            JournalClientQueryExecuter clientQueryExecuter = new JournalClientQueryExecuter(new Uri("http://localhost:5000/"));
            JObject response = await clientQueryExecuter.ExecuteQuery<JObject>("Account", "Auth", new string[] { $"login={login}", $"password={password}" });

            // Метод проверки и выброса исключения.
            _CheckAuthMethodResponse(response);

            string token = response["token"]?.ToObject<string>() ?? string.Empty,
                    refreshToken = response["refreshToken"]?.ToObject<string>() ?? string.Empty;
            DateTime tokenExpire = response["tokenExpire"]?.ToObject<DateTime>() ?? DateTime.MinValue
                , refreshTokenExpire = response["refreshTokenExpire"]?.ToObject<DateTime>() ?? DateTime.MinValue;
            clientQueryExecuter.JwtToken = token;
            User user = await clientQueryExecuter.ExecuteQuery<User>("Users", "GetMe", Array.Empty<string>());

            JournalClient journalClient = new JournalClient(
                currentUser: user
                ,  token: token
                , tokenExpire: tokenExpire
                , refreshToken: refreshToken
                , refreshTokenExpire: refreshTokenExpire);
            journalClient.CurrentUser = user;
            return journalClient;
        }

        /// <summary>
        ///     Метод проверки ответа от сервера при вызове метода Account/Auth
        /// </summary>
        /// <exception cref="ExecuteQueryException"/>
        private static void _CheckAuthMethodResponse(JObject authMethodResponse)
        {
            if (!authMethodResponse.ContainsKey("token"))
                throw new ExecuteQueryException("Неверный ответ от метода авторизации. Токен был null.");
            else if (!authMethodResponse.ContainsKey("refreshToken"))
                throw new ExecuteQueryException("Неверный ответ от метода авторизации. Токен обновления был null.");
            else if (!authMethodResponse.ContainsKey("tokenExpire"))
                throw new ExecuteQueryException("Неверный ответ от метода авторизации. Время истечения токена было null.");
            else if (!authMethodResponse.ContainsKey("refreshTokenExpire"))
                throw new ExecuteQueryException("Неверный ответ от метода авторизации. Время истечения токена обновления было null.");
        }

        
    }

    public class User : IUser
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public UserRole Role { get; set; }
    }
}
