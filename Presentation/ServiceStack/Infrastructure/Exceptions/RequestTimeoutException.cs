using ServiceStack;
using System;
using System.Net;

namespace Demo.Presentation.ServiceStack.Infrastructure.Exceptions
{
    public class RequestTimeoutException : Exception, IHasStatusCode
    {
        public int StatusCode => (int)HttpStatusCode.RequestTimeout;
    }
}
