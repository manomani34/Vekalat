using System;
using System.Collections.Generic;
using System.Net;
using Vekalat.Core.Localization;
namespace Vekalat.Core.Errors
{
    public class WebAppException : Exception
    {
        public WebAppException(HttpStatusCode errorCode, KeyValuePair<string, string> error)
        {
            ErrorCode = errorCode;
            Error = error;
        }

        public HttpStatusCode ErrorCode { get; }
        public KeyValuePair<string, string> Error { get; }
    }

    public class WebAppNotFoundException : WebAppException
    {
        public WebAppNotFoundException() : base(HttpStatusCode.NotFound,new KeyValuePair<string, string>("Not Found", Messages.EntityNotFound)) { }
    }
}
