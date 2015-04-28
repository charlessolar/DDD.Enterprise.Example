using Aggregates.Exceptions;
using Demo.Library.Extensions;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item
{
    public class Item : Aggregates.Entity<Guid>, IItem
    {
        public ValueObjects.Amount Amount { get; private set; }

        public ValueObjects.ReconciledAmount Reconciled { get; private set; }

        private Item()
        {
        }

        public void Create(DateTime Effective, String Reference, Guid AccountId, Guid PeriodId, Decimal Amount)
        {
            this.Amount = new ValueObjects.Amount(Amount);
            this.Reconciled = new ValueObjects.ReconciledAmount(0);

            Apply<Events.Created>(e =>
            {
                e.JournalId = AggregateId;
                e.Effective = Effective;
                e.Reference = Reference;
                e.AccountId = AccountId;
                e.PeriodId = PeriodId;
                e.Amount = Amount;
            });
        }

        public void ChangeEffective(DateTime Effective)
        {
            Apply<Events.EffectiveChanged>(e =>
            {
                e.JournalId = AggregateId;
                e.ItemId = Id;
                e.Effective = Effective;
            });
        }

        public void ChangeReference(String Reference)
        {
            Apply<Events.ReferenceChanged>(e =>
            {
                e.JournalId = AggregateId;
                e.ItemId = Id;
                e.Reference = Reference;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.ItemId = Id;
                e.JournalId = AggregateId;
            });
        }

        public void Reconcile(Guid OtherItemId, Decimal Amount)
        {
            var newReconciled = new ValueObjects.ReconciledAmount(this.Reconciled.Value + Amount);
            var spec = new Specifications.ReconciledAmount { MinValue = 0, MaxValue = this.Amount.Value };
            if (!spec.IsSatisfiedBy(newReconciled))
                throw new BusinessException("Reconciled {0} of total {1} change: {2}".Fmt(newReconciled.Value, this.Amount.Value, Amount));

            Apply<Events.Reconciled>(e =>
            {
                e.JournalId = AggregateId;
                e.ItemId = Id;
                e.OtherItemId = OtherItemId;
                e.Amount = Amount;
            });
        }
    }
}