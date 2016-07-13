using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Riak.Infrastructure.Exceptions
{
    public class StorageException : Exception
    {
        public StorageException() : base() { }
        public StorageException(String message) : base(message) { }
        public StorageException(String message, IEnumerable<Exception> Inners) : base(message, new AggregateException(Inners)) { }
        public StorageException(String message, Exception Inner) : base(message, Inner) { }
    }
}
