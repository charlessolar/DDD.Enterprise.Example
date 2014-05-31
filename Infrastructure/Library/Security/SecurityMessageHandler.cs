using Demo.Library.Extensions;
using Demo.Library.Queries;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public class SecurityMessageHandler : IHandleMessages<IMessage>
    {
        private readonly IBus _bus;
        private readonly IManager _manager;

        public SecurityMessageHandler(IBus bus, IManager manager)
        {
            _bus = bus;
            _manager = manager;
        }

        public void Handle(IMessage message)
        {
            String auth = "";
            _bus.CurrentMessageContext.Headers.TryGetValue("Authorization-Token", out auth);

            if (message is BasicQuery && !_manager.Authorize(auth, message.GetType().GetSecurityContext(), "Query"))
                _bus.DoNotContinueDispatchingCurrentMessageToHandlers();
            if (message is ICommand && !_manager.Authorize(auth, message.GetType().GetSecurityContext(), "Command"))
                _bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }
}