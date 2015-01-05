using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.ValueObjects
{
    public class Price : ValueObject<Price>
    {
        public readonly Decimal Amount;

        public Price(Decimal Price)
        {
            this.Amount = Price;
        }
    }
}
