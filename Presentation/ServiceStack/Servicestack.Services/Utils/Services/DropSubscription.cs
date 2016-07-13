using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/subscriptions/{SubscriptionId}", "DELETE")]
    public class DropSubscription : IReturnVoid
    {
        public String SubscriptionId { get; set; }
    }
}
