using Aggregates;
using NServiceBus;

namespace Demo.Domain.Inventory.Items.SerialNumbers
{
    public class Handler : IHandleMessages<Commands.Create>, IHandleMessages<Commands.TakeQuantity>
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
            var item = _uow.Repository<Item>().Get(command.ItemId);
            var serial = item.Entity<SerialNumber>().New(command.SerialNumberId);

            serial.Create(command.SerialNumber, command.Quantity, command.Effective, command.ItemId);
        }

        public void Handle(Commands.TakeQuantity command)
        {
            var item = _uow.Repository<Item>().Get(command.ItemId);
            var serial = item.Entity<SerialNumber>().Get(command.SerialNumberId);

            serial.TakeQuantity(command.Quantity);
        }
    }
}