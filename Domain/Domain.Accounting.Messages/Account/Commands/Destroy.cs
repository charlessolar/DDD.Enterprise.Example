using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Command;

namespace Demo.Domain.Accounting.Account.Commands
{
    public class Destroy : DemoCommand
    {
        public Guid AccountId { get; set; }
    }
}
