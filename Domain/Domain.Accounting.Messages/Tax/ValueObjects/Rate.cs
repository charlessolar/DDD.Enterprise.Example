using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.ValueObjects
{
    public class Rate : Aggregates.ValueObject<Rate>
    {
        public readonly Boolean Fixed;
        public readonly Decimal Value;

        public Rate(Boolean Fixed, Decimal Rate)
        {
            this.Fixed = Fixed;
            this.Value = Rate;
        }
    }
}