using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries
{
    public class IdResult : IMessage
    {
        public Guid Id { get; set; }
    }
}
