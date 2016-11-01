using System;

namespace Demo.Presentation.ServiceStack.Infrastructure.Exceptions
{
    public class UnexpectedResponse :Exception
    {
        public UnexpectedResponse(string message, Type expected, Type received) : base($"Message: {message} Expected: {expected.FullName} Received: {received.FullName}")
        {
        }
    }
}
