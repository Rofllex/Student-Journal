using System.Threading.Tasks;
using System.Collections.Generic;

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
}
