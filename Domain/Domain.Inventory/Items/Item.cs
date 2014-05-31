using Demo.Domain.Inventory.Items.Events;
using Demo.Library.Identity;
using NES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items
{
    public class Item : IdentityAggregateRoot<Identities.Item>
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

            _identity = new Identities.Item
            {
                Id = e.ItemId,
                Number = e.Number,
                Description = e.Description,
                UnitOfMeasure = e.UnitOfMeasure,
                CatalogPrice = e.CatalogPrice,
                CostPrice = e.CostPrice,
            };
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