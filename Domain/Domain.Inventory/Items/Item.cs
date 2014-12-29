using Demo.Domain.Inventory.Items.Events;
using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items
{
    public class Item : Aggregate<Guid>
    {
        private Item() { }
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
        }

        private void Handle(DescriptionChanged e)
        {
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