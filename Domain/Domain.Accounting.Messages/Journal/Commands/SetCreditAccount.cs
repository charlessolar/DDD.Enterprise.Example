using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Command;

namespace Demo.Domain.Accounting.Journal.Commands
{
    public class SetCreditAccount : DemoCommand
    {
        public Guid JournalId { get; set; }
        public Guid? AccountId { get; set; }
    }
}
