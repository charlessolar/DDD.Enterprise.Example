using System;
using System.Collections.Generic;

namespace Demo.Domain.Infrastructure.Exceptions
{
    public class StorageException : Exception
    {
        public StorageException() : base() { }
        public StorageException(string message) : base(message) { }
        public StorageException(string message, IEnumerable<Exception> inners) : base(message, new AggregateException(inners)) { }
        public StorageException(string message, Exception inner) : base(message, inner) { }
    }
}
