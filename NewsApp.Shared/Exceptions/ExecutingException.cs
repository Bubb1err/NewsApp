﻿using System.Net;

namespace Analytics.Web.Exceptions
{
    public class ExecutingException : Exception
    {
        public ExecutingException(string errorMessage, HttpStatusCode code)
        {
            ErrorMessage = errorMessage;
            StatusCode = code;
        }

        public string ErrorMessage { get; }

        public HttpStatusCode StatusCode { get; }
    }
}
