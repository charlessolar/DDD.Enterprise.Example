using Demo.Library.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.Identities
{
    public class Item : IIdentity
    {
        public Guid Id { get; set; }
        public String Number { get; set; }
        public String Description { get; set; }

        public String UnitOfMeasure { get; set; }

        public Decimal? CatalogPrice { get; set; }
        public Decimal? CostPrice { get; set; }
    }
}