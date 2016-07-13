using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Exceptions
{
    public class QueryRejectedException : Exception
    {
        public QueryRejectedException()
        {
        }

        public QueryRejectedException(String message)
            : base(message)
        {
        }

        public QueryRejectedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
