using Domain.Inventory.Items.Events;
using NES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.Items
{
    public class Item : AggregateBase
    {
        public Item(Guid ItemId, String Number, String Description, String UnitOfMeasure, Decimal? CatalogPrice, Decimal? CostPrice)
        {
            Apply<Created>(e =>
                {
                    e.ItemId = ItemId;
                    e.Number = Number;
                    e.Description = Description;
                    e.UnitOfMeasure = UnitOfMeasure;
                    e.CatalogPrice = CatalogPrice;
                    e.CostPrice = CostPrice;
                });
        }

        private Item()
        {
        }

        private void Handle(Created e)
        {
            Id = e.ItemId;
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