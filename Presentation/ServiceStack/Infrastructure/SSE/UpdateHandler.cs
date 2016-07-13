using Aggregates;
using NServiceBus;
using Demo.Library.Updates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public class UpdateHandler : IHandleMessagesAsync<Update>
    {
        private readonly ISubscriptionManager _manager;

        public UpdateHandler(ISubscriptionManager manager)
        {
            _manager = manager;
        }

        public Task Handle(Update e, IHandleContext ctx)
        {
            return _manager.Publish(e.Payload, e.ChangeType);
        }
    }
}
