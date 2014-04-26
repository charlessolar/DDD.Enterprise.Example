using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.Items.Events
{
    public interface Created : IEvent
    {
        Guid ItemId { get; set; }

        String Number { get; set; }
        String Description { get; set; }

        String UnitOfMeasure { get; set; }

        Decimal? CatalogPrice { get; set; }
        Decimal? CostPrice { get; set; }
    }
}