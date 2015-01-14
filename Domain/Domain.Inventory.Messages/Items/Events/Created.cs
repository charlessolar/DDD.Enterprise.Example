using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.Events
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