using NServiceBus;
using StructureMap;
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
        private readonly IContainer _container;
        private readonly IManager _manager;

        public SecurityMessageHandler(IBus bus, IContainer container, IManager manager)
        {
            _bus = bus;
            _container = container;
            _manager = manager;
        }

        public void Handle(IMessage message)
        {
            var result = _manager.Authorize<Actions.ReceivingAction>(message);
            if (!result.IsAuthorized)
                _bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }
}