using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries
{
    public class Result : IMessage
    {
        public IEnumerable<Object> Records { get; set; }
    }
}
