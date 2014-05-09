using NServiceBus;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Authorization
{
    public class AuthorizationMessageHandler : IHandleMessages<IMessage>
    {
        private readonly IBus _bus;
        private readonly IContainer _container;

        public AuthorizationMessageHandler(IBus bus, IContainer container)
        {
            _bus = bus;
            _container = container;
        }

        public void Handle(IMessage message)
        {
            return;
        }
    }
}