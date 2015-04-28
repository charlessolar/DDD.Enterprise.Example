using Demo.Library.Extensions;
using Demo.Library.Security;
using NServiceBus;
using System;

namespace Demo.Domain.Security
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
        }
    }
}