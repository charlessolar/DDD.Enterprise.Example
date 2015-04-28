using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.Commands
{
    public class Deactivate : DemoCommand
    {
        public Guid TaxId { get; set; }
    }
}