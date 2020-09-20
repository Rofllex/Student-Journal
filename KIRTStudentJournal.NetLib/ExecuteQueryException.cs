using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KIRTStudentJournal.NetLib
{
    /// <summary>
    /// Исключение при ошибке выполнения запроса.
    /// </summary>
    public class ExecuteQueryException : Exception
    {
        public HttpStatusCode? StatusCode { get; protected set; }

        public ExecuteQueryException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public ExecuteQueryException(Exception inner) : base(message: null, innerException: inner)
        {
        }

        protected ExecuteQueryException()
        {
        }
    }

    /// <summary>
    /// Исключение при возвращении ошибки с сервера.
    /// </summary>
    public class RequestErrorException : ExecuteQueryException
    {
        /// <summary>
        /// Модель ошибки возвращаемой с сервера.
        /// </summary>
        public Models.Error Error { get; private set; }
        /// <summary>
        /// Конструктор класса <see cref="RequestErrorException"/>
        /// </summary>
        public RequestErrorException(Models.Error error, HttpStatusCode? statusCode = null)
        {
            Error = error;
            StatusCode = statusCode;
        }

        public RequestErrorException()
        {
        }
    }

}
