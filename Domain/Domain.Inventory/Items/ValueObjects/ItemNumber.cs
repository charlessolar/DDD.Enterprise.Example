using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.ValueObjects
{
    public class ItemNumber : ValueObject<ItemNumber>
    {
        public readonly String Number;

        public ItemNumber(String Number)
        {
            this.Number = Number;
        }
    }
}
