using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.ValueObjects
{
    public class CheckDate : Aggregates.ValueObject<CheckDate>
    {
        public readonly Boolean Value;

        public CheckDate(Boolean CheckDate)
        {
            this.Value = CheckDate;
        }
    }
}