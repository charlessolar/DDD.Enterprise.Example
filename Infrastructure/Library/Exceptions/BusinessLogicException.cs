using System;

namespace Demo.Library.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException()
        {
        }

        public BusinessLogicException(String message)
            : base(message)
        {
        }

        public BusinessLogicException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}