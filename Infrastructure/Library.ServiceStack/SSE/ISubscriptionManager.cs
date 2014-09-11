using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Library.SSE
{
    public interface ISubscriptionManager
    {
        Boolean IsSubscribed(String Session, String Domain);
        ISet<String> GetSubscriptions(String Domain);
        void Subscribe(String Session, String Domain);
        void Unsubscribe(String Session, String Domain);
    }
}
