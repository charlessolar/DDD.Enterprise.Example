using ServiceStack;

namespace Demo.Library.Responses
{
    public class Base : IHasResponseStatus
    {
        public ResponseStatus ResponseStatus { get; set; }
    }

    public class Envelope<T> : Base
    {
        public T Payload { get; set; }
    }
}