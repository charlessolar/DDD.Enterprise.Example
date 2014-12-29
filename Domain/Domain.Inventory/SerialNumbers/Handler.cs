using Aggregates;
using Demo.Library.Queries;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.SerialNumbers
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
            var serial = _uow.For<SerialNumber>().New(command.SerialNumberId);
            
            serial.Create(command.SerialNumber, command.Quantity, command.Effective, command.ItemId);

            _bus.Reply<IdResult>(e =>
            {
                e.Id = serial.Id;
            });
        }

        public void Handle(Commands.TakeQuantity command)
        {
            var serial = _uow.For<SerialNumber>().Get(command.SerialNumberId);
            serial.TakeQuantity(command.Quantity);
        }
    }
}