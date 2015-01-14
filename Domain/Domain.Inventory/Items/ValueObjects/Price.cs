using Aggregates;
using System;

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