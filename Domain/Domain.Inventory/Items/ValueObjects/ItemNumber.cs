using Aggregates;
using System;

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