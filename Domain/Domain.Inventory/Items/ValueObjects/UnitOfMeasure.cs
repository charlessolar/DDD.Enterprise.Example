using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
