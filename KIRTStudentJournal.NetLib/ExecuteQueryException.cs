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
        public HttpStatusCode? StatusCode { get; private set; }

        public ExecuteQueryException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public ExecuteQueryException(Exception inner) :
            base(message: null
            , innerException: inner)
        {
        }

        protected ExecuteQueryException()
        {
        }
    }

    public class RequestErrorException : ExecuteQueryException
    {
        public Models.Error Error { get; private set; }
        public RequestErrorException(Models.Error error)
        {
            Error = error;
        }
    }

}
