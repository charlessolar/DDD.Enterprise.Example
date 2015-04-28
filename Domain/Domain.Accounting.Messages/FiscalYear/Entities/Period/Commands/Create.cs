using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period.Commands
{
    public class Create : DemoCommand
    {
        public Guid FiscalYearId { get; set; }

        public Guid PeriodId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }
    }
}