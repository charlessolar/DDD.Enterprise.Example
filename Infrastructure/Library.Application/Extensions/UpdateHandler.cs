using Aggregates;
using NServiceBus;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public class UpdateHandler :
        IHandleMessagesAsync<Updates.Update>
    {
        private readonly ISubscriptionManager _manager;

        public UpdateHandler(ISubscriptionManager manager)
        {
            _manager = manager;
        }

        public Task Handle(Updates.Update e, IHandleContext ctx)
        {
            // Todo: Etags and timestamp
            _manager.Publish(e.Payload, e.ChangeType);

            return Task.FromResult(0);
        }
    }
}
