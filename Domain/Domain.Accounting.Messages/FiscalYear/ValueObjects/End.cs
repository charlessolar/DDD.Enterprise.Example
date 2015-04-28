using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.ValueObjects
{
    public class End : Aggregates.ValueObject<End>
    {
        public readonly DateTime? Date;

        public End(DateTime? End = null)
        {
            this.Date = End;
        }
    }
}