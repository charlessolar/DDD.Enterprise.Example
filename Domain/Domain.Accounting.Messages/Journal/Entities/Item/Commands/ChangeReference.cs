using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Commands
{
    public class ChangeReference : DemoCommand
    {
        public Guid JournalId { get; set; }

        public Guid ItemId { get; set; }

        public String Reference { get; set; }
    }
}