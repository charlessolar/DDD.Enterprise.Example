using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period.ValueObjects
{
    public class Start : Aggregates.ValueObject<Start>
    {
        public readonly DateTime? Date;

        public Start(DateTime? Start = null)
        {
            this.Date = Start;
        }
    }
}