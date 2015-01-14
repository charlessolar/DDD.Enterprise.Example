using Aggregates;
using System;

namespace Demo.Domain.Inventory.Items.SerialNumbers.ValueObjects
{
    public class Effective : ValueObject<Effective>
    {
        public readonly DateTime Date;

        public Effective(DateTime Effective)
        {
            this.Date = Effective;
        }
    }
}