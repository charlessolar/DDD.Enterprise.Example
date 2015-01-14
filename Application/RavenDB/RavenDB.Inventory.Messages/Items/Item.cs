using System;

namespace Demo.Application.RavenDB.Inventory.Items
{
    public class Item
    {
        public Guid Id { get; set; }

        public String Number { get; set; }

        public String Description { get; set; }

        public String UnitOfMeasure { get; set; }

        public Decimal? CatalogPrice { get; set; }

        public Decimal? CostPrice { get; set; }
    }
}