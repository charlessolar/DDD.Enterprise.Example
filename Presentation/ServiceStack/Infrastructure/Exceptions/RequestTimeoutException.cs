using ServiceStack;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.Exceptions
{
    public class RequestTimeoutException : Exception, IHasStatusCode
    {
        public int StatusCode
        {
            get { return (int)HttpStatusCode.RequestTimeout; }
        }
    }
}
