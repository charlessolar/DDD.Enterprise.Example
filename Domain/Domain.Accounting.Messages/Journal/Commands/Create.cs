using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Command;
namespace Demo.Domain.Accounting.Journal.Commands
{
    public class Create : DemoCommand
    {
        public Guid JournalId { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }

        public Guid ResponsibleId { get; set; }

        public Boolean CheckDate { get; set; }
        public Boolean SkipDraft { get; set; }
    }
}
