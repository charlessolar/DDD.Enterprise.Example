using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Region.Entities.Match
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.ChangeValue>,
        IHandleMessages<Commands.ChangeType>,
        IHandleMessages<Commands.ChangeOperation>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Create command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            var match = region.E<Match>().New(command.MatchId);
            match.Create(command.Value, command.Type, command.Operation);
        }

        public void Handle(Commands.Destroy command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            var match = region.E<Match>().Get(command.MatchId);
            match.Destroy();
        }

        public void Handle(Commands.ChangeOperation command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            var match = region.E<Match>().Get(command.MatchId);
            match.ChangeOperation(command.Operation);
        }

        public void Handle(Commands.ChangeType command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            var match = region.E<Match>().Get(command.MatchId);
            match.ChangeType(command.Type);
        }

        public void Handle(Commands.ChangeValue command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            var match = region.E<Match>().Get(command.MatchId);
            match.ChangeValue(command.Value);
        }
    }
}