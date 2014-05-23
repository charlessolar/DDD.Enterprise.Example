using NServiceBus;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Hows
{
    // Authorize a message to receive
    public class Receiving : IHow, IHandleMessages<IMessage>
    {
        private readonly IBus _bus;
        private readonly IContainer _container;
        private IList<IWhat> _whats;

        public Receiving(IBus bus, IContainer container)
        {
            _bus = bus;
            _container = container;
            _whats = new List<IWhat>();
        }

        public void Handle(IMessage message)
        {
            //var result = _manager.Authorize(message);
            //if (!result.IsAuthorized)
            //    _bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }

        public String Description { get; set; }
        public void AddWhat(IWhat what)
        {
            _whats.Add(what);
        }
    }
}