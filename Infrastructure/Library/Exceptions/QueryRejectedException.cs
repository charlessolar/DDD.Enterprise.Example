using System;

namespace Demo.Library.Exceptions
{
    public class QueryRejectedException : Exception
    {
        public QueryRejectedException()
        {
        }

        public QueryRejectedException(string message)
            : base(message)
        {
        }

        public QueryRejectedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
