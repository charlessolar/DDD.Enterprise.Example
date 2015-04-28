using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Entry.Commands
{
    public class RequestReview : DemoCommand
    {
        public Guid JournalId { get; set; }

        public Guid EntryId { get; set; }

        public Guid? ReviewerId { get; set; }
    }
}