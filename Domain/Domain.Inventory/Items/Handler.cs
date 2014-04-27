using NES;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.Items
{
    public class Handler : IHandleMessages<Commands.Create>, IHandleMessages<Commands.ChangeDescription>
    {
        private readonly IRepository _repository;


        public Handler(IRepository repository)
        {
            _repository = repository;
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
        }

        public void Handle(Commands.ChangeDescription command)
        {
            var item = _repository.Get<Item>(command.ItemId);
            item.ChangeDescription(command.Description);
        }
    }
}