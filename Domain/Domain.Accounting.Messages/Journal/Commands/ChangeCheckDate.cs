using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Command;

namespace Demo.Domain.Accounting.Journal.Commands
{
    public class ChangeCheckDate : DemoCommand
    {
        public Guid JournalId { get; set; }
        public Boolean CheckDate { get; set; }
    }
}
