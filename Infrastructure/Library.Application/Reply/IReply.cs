using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Reply
{


    // Used when replying to a query
    public interface IReply : IMessage
    {
        string ETag { get; set; }

        Object Payload { get; set; }
    }
    public interface IPagedReply : IMessage
    {
        long ElapsedMs { get; set; }

        long Total { get; set; }
        
        IEnumerable<Object> Records { get; set; }
    }
}
