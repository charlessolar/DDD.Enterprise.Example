using NServiceBus;
using System.Collections.Generic;

namespace Demo.Library.Reply
{


    // Used when replying to a query
    public interface IReply : IMessage
    {
        string ETag { get; set; }

        object Payload { get; set; }
    }
    public interface IPagedReply : IMessage
    {
        long ElapsedMs { get; set; }

        long Total { get; set; }
        
        IEnumerable<object> Records { get; set; }
    }
}
