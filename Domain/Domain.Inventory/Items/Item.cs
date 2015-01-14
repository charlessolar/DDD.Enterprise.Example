using Aggregates;
using Demo.Domain.Inventory.Items.Events;
using System;

namespace Demo.Domain.Inventory.Items
{
    public class Item : Aggregate<Guid>
    {
        public ValueObjects.ItemNumber ItemNumber { get; private set; }

        public ValueObjects.Description Description { get; private set; }

        public ValueObjects.UnitOfMeasure UnitOfMeasure { get; private set; }

        public ValueObjects.Price CatalogPrice { get; private set; }

        public ValueObjects.Price CostPrice { get; private set; }

        private Item()
        {
        }

        public void Create(String Number, String Description, String UnitOfMeasure, Decimal? CatalogPrice, Decimal? CostPrice)
        {
            Apply<Created>(e =>
            {
                e.ItemId = Id;
                e.Number = Number;
                e.Description = Description;
                e.UnitOfMeasure = UnitOfMeasure;
                e.CatalogPrice = CatalogPrice;
                e.CostPrice = CostPrice;
            });
        }

        private void Handle(Created e)
        {
            this.ItemNumber = new ValueObjects.ItemNumber(e.Number);
            this.Description = new ValueObjects.Description(e.Description);
            this.UnitOfMeasure = new ValueObjects.UnitOfMeasure(e.UnitOfMeasure);
            this.CatalogPrice = new ValueObjects.Price(e.CatalogPrice ?? 0);
            this.CostPrice = new ValueObjects.Price(e.CostPrice ?? 0);
        }

        private void Handle(DescriptionChanged e)
        {
            this.Description = new ValueObjects.Description(e.Description);
        }

        public void ChangeDescription(String Description)
        {
            Apply<DescriptionChanged>(e =>
            {
                e.ItemId = this.Id;
                e.Description = Description;
            });
        }
    }
}