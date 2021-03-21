using System.Threading.Tasks;
using System.Collections.Generic;

#nullable enable

namespace Journal.ClientLib.Infrastructure
{
    /// <summary>
    ///     Абстракция выполнения запроса.
    /// </summary>
    public interface IClientQueryExecuter
    {
        /// <summary>
        ///     Выполнить GET запрос.
        /// </summary>
        /// <typeparam name="T">
        ///     Тип, к которому необходимо привести ответ.
        /// </typeparam>
        /// <param name="controller">
        ///     Контроллер к которому необходимо обратиться
        /// </param>
        /// <param name="method">
        ///     Метод контроллера.
        /// </param>
        /// <param name="getArgs">
        ///     GET аргументы
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации
        /// </param>
        /// <returns></returns>
        Task<T> ExecuteGetQuery<T>(string controller, string method, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true);
        
        /// <summary>
        ///     Выполнить GET запрос без парсинга ответа
        /// </summary>
        /// <param name="controller">
        ///     Контроллер к которому необходимо обратиться
        /// </param>
        /// <param name="method">
        ///     Метод контроллера
        /// </param>
        /// <param name="getArgs">
        ///     GET аргументы
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации
        /// </param>
        /// <returns></returns>
        Task ExecuteGetQuery(string controller, string method, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true);

        /// <summary>
        ///     Выполнить POST запрос.
        /// </summary>
        /// <typeparam name="T">
        ///     Тип к которому необходимо привести ответ
        /// </typeparam>
        /// <param name="controller">
        ///     Контроллер к которому следует обратиться
        /// </param>
        /// <param name="method">
        ///     Метод контроллера
        /// </param>
        /// <param name="postBody">
        ///     Тело POST запроса
        /// </param>
        /// <param name="getArgs">
        ///     GET аргументы запроса
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации
        /// </param>
        /// <returns></returns>
        Task<T> ExecutePostQuery<T>(string controller, string method, object? postBody, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true);
        
        /// <summary>
        ///     Выполнить POST запрос
        /// </summary>
        /// <param name="controller">
        ///     Контроллер к которому следует обратиться
        /// </param>
        /// <param name="method">
        ///     Метод контроллера
        /// </param>
        /// <param name="postBody">
        ///     Тело POST запроса
        /// </param>
        /// <param name="getArgs">
        ///     GET аргументы запроса
        /// </param>
        /// <param name="useToken">
        ///     Если необходимо использовать токен авторизации
        /// </param>
        /// <returns></returns>
        Task ExecutePostQuery(string controller, string method, object? postBody, IEnumerable<KeyValuePair<string, string>>? getArgs = null, bool useToken = true);
    }
}
