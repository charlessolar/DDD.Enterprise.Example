using System;

namespace Demo.Library.Exceptions
{
    public class SecurityException : Exception
    {
        public SecurityException()
        {
        }

        public SecurityException(String message)
            : base(message)
        {
        }

        public SecurityException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}