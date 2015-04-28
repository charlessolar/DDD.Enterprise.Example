using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Entry
{
    public class Entry : Aggregates.Entity<Guid>, IEntry
    {
        public Aggregates.SingleValueObject<Boolean> NeedsReview { get; private set; }

        private Entry()
        {
            this.NeedsReview = new Aggregates.SingleValueObject<bool>(false);
        }

        public void RequestReview(Guid? EmployeeId)
        {
            Apply<Events.ReviewRequested>(e =>
            {
                e.JournalId = AggregateId;
                e.EntryId = Id;
                e.EmployeeId = EmployeeId;
            });
        }

        private void Handle(Events.ReviewRequested e)
        {
            this.NeedsReview = new Aggregates.SingleValueObject<bool>(true);
        }

        public void Reviewed(Guid EmployeeId)
        {
            Apply<Events.Reviewed>(e =>
            {
                e.JournalId = AggregateId;
                e.EntryId = Id;
                e.EmployeeId = EmployeeId;
            });
        }

        private void Handle(Events.Reviewed e)
        {
            this.NeedsReview = new Aggregates.SingleValueObject<bool>(false);
        }
    }
}