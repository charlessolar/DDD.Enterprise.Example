using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.TaxType.Commands
{
    public class Create : DemoCommand
    {
        public Guid TaxTypeId { get; set; }

        public String Name { get; set; }
    }
}