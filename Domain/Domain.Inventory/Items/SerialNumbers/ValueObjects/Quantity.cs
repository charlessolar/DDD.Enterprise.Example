using Aggregates;
using System;

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