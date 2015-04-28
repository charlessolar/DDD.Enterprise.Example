using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Commands
{
    public class ChangeResponsible : DemoCommand
    {
        public Guid JournalId { get; set; }

        public Guid ResponsibleId { get; set; }
    }
}