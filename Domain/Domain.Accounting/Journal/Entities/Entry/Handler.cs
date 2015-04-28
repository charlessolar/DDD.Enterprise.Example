using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Entry
{
    public class Handler :
        IHandleMessages<Commands.RequestReview>,
        IHandleMessages<Commands.Reviewed>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.RequestReview command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            var entry = journal.Entity<Entry>().Get(command.EntryId);

            entry.RequestReview(command.ReviewerId);
        }

        public void Handle(Commands.Reviewed command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            var entry = journal.Entity<Entry>().Get(command.EntryId);

            entry.Reviewed(command.ReviewerId);
        }
    }
}