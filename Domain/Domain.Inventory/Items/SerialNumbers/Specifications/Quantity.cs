using Aggregates.Specifications;
using System;
using System.Linq.Expressions;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Specifications
{
    public class Quantity : Specification<ValueObjects.Quantity>
    {
        public Int32 MinValue { get; set; }

        public override Expression<Func<ValueObjects.Quantity, Boolean>> Predicate
        {
            get
            {
                return a => a.Total > MinValue;
            }
        }
    }
}