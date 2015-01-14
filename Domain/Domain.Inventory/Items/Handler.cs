using Aggregates;
using NServiceBus;

namespace Demo.Domain.Inventory.Items
{
    public class Handler : IHandleMessages<Commands.Create>, IHandleMessages<Commands.ChangeDescription>
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
            var item = _uow.For<Item>().New(command.ItemId);
            item.Create(
                command.Number,
                command.Description,
                command.UnitOfMeasure,
                command.CatalogPrice,
                command.CostPrice
                );
        }

        public void Handle(Commands.ChangeDescription command)
        {
            var item = _uow.For<Item>().Get(command.ItemId);
            item.ChangeDescription(command.Description);
        }
    }
}