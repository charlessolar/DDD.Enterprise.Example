using Aggregates.Specifications;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Specifications
{
    public class SerialNumber : Specification<ValueObjects.SerialNumber>
    {
        public Int32 Count { get; set; }

        public override Expression<Func<ValueObjects.SerialNumber, Boolean>> Predicate
        {
            get
            {
                // Must contain X -'s specification
                return a => a.Serial.Where(c => c == '-').Count() == Count;
            }
        }
    }
}