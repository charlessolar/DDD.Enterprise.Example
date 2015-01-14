using Aggregates;
using System;

namespace Demo.Domain.Inventory.Items.ValueObjects
{
    public class UnitOfMeasure : ValueObject<UnitOfMeasure>
    {
        public readonly String UOM;

        public UnitOfMeasure(String UnitOfMeasure)
        {
            this.UOM = UnitOfMeasure;
        }
    }
}