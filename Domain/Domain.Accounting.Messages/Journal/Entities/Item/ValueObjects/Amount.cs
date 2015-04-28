using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.ValueObjects
{
    public class Amount : Aggregates.ValueObject<Amount>
    {
        public readonly Decimal Value;

        public Amount(Decimal Reconciled)
        {
            this.Value = Reconciled;
        }
    }
}