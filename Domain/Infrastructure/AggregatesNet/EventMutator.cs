using Aggregates.Contracts;
using NServiceBus;
using Demo.Library.Command;
using System.Collections.Generic;

namespace Demo.Domain.Infrastructure.AggregatesNet
{
    public class EventMutator : IEventMutator
    {
        private readonly Aggregates.IUnitOfWork _uow;

        public EventMutator(Aggregates.IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEvent MutateIncoming(IEvent Event, IReadOnlyDictionary<string, string> headers)
        {            
            return Event;
        }

        public IEvent MutateOutgoing(IEvent Event)
        {

            if (!(Event is IDemoEvent)) return Event;
            if (!(_uow.CurrentMessage is DemoCommand) && !(_uow.CurrentMessage is IDemoEvent)) return Event;

            var pulseEvent = Event as IDemoEvent;
            if (_uow.CurrentMessage is DemoCommand)
            {
                var command = _uow.CurrentMessage as DemoCommand;
                pulseEvent.CurrentUserId = command.CurrentUserId;
                pulseEvent.Stamp = command.Stamp;
            }
            if (_uow.CurrentMessage is IDemoEvent)
            {
                var @event = _uow.CurrentMessage as IDemoEvent;
                pulseEvent.CurrentUserId = @event.CurrentUserId;
                pulseEvent.Stamp = @event.Stamp;
            }
            
            Event = pulseEvent;
            
            return Event;
        }
    }
}
