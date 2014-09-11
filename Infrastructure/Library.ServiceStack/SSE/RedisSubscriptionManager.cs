using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Infrastructure.Library.SSE
{
    public class RedisSubscriptionManager : ISubscriptionManager
    {
        public Boolean IsSubscribed(String Session, String Domain)
        {
            throw new NotImplementedException();
        }
        public ISet<String> GetSubscriptions(String Domain)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(String Session, String Domain)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(String Session, String Domain)
        {
            throw new NotImplementedException();
        }


    }
}