using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Responses
{
    public class Base<T> : IHasResponseStatus, IResponse
    {
        public T Payload { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}