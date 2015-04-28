using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.ValueObjects
{
    public class ReconciledAmount : Aggregates.ValueObject<ReconciledAmount>
    {
        public readonly Decimal Value;

        public ReconciledAmount(Decimal Reconciled)
        {
            this.Value = Reconciled;
        }
    }
}