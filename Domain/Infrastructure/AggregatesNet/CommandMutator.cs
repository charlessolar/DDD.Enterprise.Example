using Aggregates.Contracts;
using NServiceBus;
using Demo.Library.Command;
using System.Collections.Generic;

namespace Demo.Domain.Infrastructure.AggregatesNet
{
    public class CommandMutator : ICommandMutator
    {
        private readonly Aggregates.IUnitOfWork _uow;

        public CommandMutator(Aggregates.IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ICommand MutateIncoming(ICommand command, IReadOnlyDictionary<string, string> headers)
        {

            return command;
        }

        public ICommand MutateOutgoing(ICommand command)
        {
            if (!(command is DemoCommand)) return command;
            if (!(_uow.CurrentMessage is DemoCommand) && !(_uow.CurrentMessage is IDemoEvent)) return command;
            
            if (_uow.CurrentMessage is DemoCommand)
            {
                var current = (DemoCommand) _uow.CurrentMessage;
                ((DemoCommand) command).CurrentUserId = current.CurrentUserId;
                ((DemoCommand) command).Stamp = current.Stamp;
            }
            if (_uow.CurrentMessage is IDemoEvent)
            {
                var @event = _uow.CurrentMessage as IDemoEvent;
                ((DemoCommand) command).CurrentUserId = @event.CurrentUserId;
                ((DemoCommand) command).Stamp = @event.Stamp;
            }

            return command;
        }
    }
}
