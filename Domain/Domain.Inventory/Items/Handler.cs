using Demo.Library.Queries;
using NES;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items
{
    public class Handler : IHandleMessages<Commands.Create>, IHandleMessages<Commands.ChangeDescription>
    {
        private readonly IRepository _repository;
        private readonly IBus _bus;

        public Handler(IRepository repository, IBus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        public void Handle(Commands.Create command)
        {
            var item = new Item(
                command.ItemId,
                command.Number,
                command.Description,
                command.UnitOfMeasure,
                command.CatalogPrice,
                command.CostPrice
                );
            _repository.Add(item);

            _bus.Reply<IdResult>(e =>
            {
                e.Id = item.Id;
            });
        }

        public void Handle(Commands.ChangeDescription command)
        {
            var item = _repository.Get<Item>(command.ItemId);
            item.ChangeDescription(command.Description);

            _bus.Reply<IdResult>(e =>
            {
                e.Id = item.Id;
            });
        }
    }
}