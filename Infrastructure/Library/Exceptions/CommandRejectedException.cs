using System;

namespace Demo.Library.Exceptions
{
    public class CommandRejectedException : ArgumentException
    {
        public CommandRejectedException()
        {
        }

        public CommandRejectedException(String message)
            : base(message)
        {
        }

        public CommandRejectedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}