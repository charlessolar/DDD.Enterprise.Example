using NES;
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
        private readonly IRepository _repository;

        public Handler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(Commands.Create command)
        {
            var serial = new SerialNumber(
                command.SerialNumberId,
                command.SerialNumber,
                command.Quantity,
                command.Effective,
                command.ItemId
                );
            _repository.Add(serial);
        }

        public void Handle(Commands.TakeQuantity command)
        {
            var serial = _repository.Get<SerialNumber>(command.SerialNumberId);
            serial.TakeQuantity(command.Quantity);
        }
    }
}