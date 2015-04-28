using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Command;

namespace Demo.Domain.Accounting.Tax.Commands
{
    public class AddStore : DemoCommand
    {
        public Guid TaxId { get; set; }

        public Guid StoreId { get; set; }
    }
}