using Application.Inventory.Items;
using NServiceBus;
using Presentation.Inventory.Items.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Inventory.Items
{
    public class Items : Service
    {
        public IBus _bus { get; set; }


        public Item Any(GetItem request)
        {
            _bus.Send("application", new Application.Inventory.Items.Queries.GetItem
            {
                Id = request.Id
            });
            return null;
        }

        public List<Item> Any(FindItems request)
        {
            _bus.Send("application", new Application.Inventory.Items.Queries.FindItems
            {
                Number = request.Number,
                Description = request.Description,
            });

            return new List<Item>();
        }

        public Guid Post(CreateItem request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.Create>();
            command.ItemId = Guid.NewGuid();
            _bus.Send("domain", command);

            return command.ItemId;
        }
    }
}