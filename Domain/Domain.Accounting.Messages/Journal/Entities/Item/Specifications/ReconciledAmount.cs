using Aggregates.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Specifications
{
    public class ReconciledAmount : Specification<ValueObjects.ReconciledAmount>
    {
        public Decimal MinValue { get; set; }

        public Decimal MaxValue { get; set; }

        public override Expression<Func<ValueObjects.ReconciledAmount, Boolean>> Predicate
        {
            get
            {
                return a => a.Value >= MinValue && a.Value <= MaxValue;
            }
        }
    }
}