using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.SerialNumbers.ValueObjects
{
    public class Quantity : ValueObject<Quantity>
    {
        public readonly Decimal Total;

        public Quantity(Decimal Quantity)
        {
            this.Total = Quantity;
        }
    }
}
