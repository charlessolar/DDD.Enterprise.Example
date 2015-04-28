using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Command;

namespace Demo.Domain.Accounting.FiscalYear.Commands
{
    public class Start : DemoCommand
    {
        public Guid FiscalYearId { get; set; }

        public DateTime Effective { get; set; }
    }
}