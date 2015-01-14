using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.Commands
{
    public class Create : ICommand
    {
        public Guid ItemId { get; set; }

        public String Number { get; set; }

        public String Description { get; set; }

        public String UnitOfMeasure { get; set; }

        public Decimal? CatalogPrice { get; set; }

        public Decimal? CostPrice { get; set; }
    }
}