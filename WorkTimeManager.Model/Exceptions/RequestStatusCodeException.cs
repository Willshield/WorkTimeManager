using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Model.Exceptions
{
    public class RequestStatusCodeException : Exception
    {
        HttpStatusCode StatusCode = HttpStatusCode.InternalServerError;
        bool Unexpected = false;

        public RequestStatusCodeException(string message, bool unexpected = false) : base(message)
        {
            Unexpected = unexpected;
        }

        public RequestStatusCodeException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }

        public string GetErrorMessage()
        {
            if(Unexpected)
                return string.Join(" Reason: ", Message, "Unexpected error. Check internet connection, url and key.");

            switch (StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    return string.Join(" Reason: ", Message, "Permission denied.");
                case HttpStatusCode.Unauthorized:
                    return string.Join(" Reason: ", Message, "Authentication failed. Check connection key or the url.");
                case HttpStatusCode.ServiceUnavailable:
                    return string.Join(" Reason: ", Message, "Service is currently unavailable.");
                case HttpStatusCode.NotFound:
                    return string.Join(" Reason: ", Message, "Requested resource is not found.");
                case HttpStatusCode.InternalServerError:
                    return string.Join(" Reason: ", Message, "Internal server error. Request failed.");
                default:
                    return string.Join(" Reason: ", Message, "Unexpected error. Check internet connection, url and key.");
            }
        }
    }
}
