using Aggregates;
using NServiceBus;

namespace Demo.Domain.Configuration.Region
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangeDescription>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.ChangeName command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            region.ChangeName(command.Name);
        }

        public void Handle(Commands.Create command)
        {
            var region = _uow.R<Region>().New(command.RegionId);
            region.Create(command.Code, command.Name, command.ParentId);
        }

        public void Handle(Commands.Destroy command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            region.Destroy();
        }

        public void Handle(Commands.ChangeDescription command)
        {
            var region = _uow.R<Region>().Get(command.RegionId);
            region.ChangeDescription(command.Description);
        }
    }
}