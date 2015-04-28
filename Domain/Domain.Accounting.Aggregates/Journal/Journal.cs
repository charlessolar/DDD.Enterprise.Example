using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal
{
    public class Journal : Aggregates.Aggregate<Guid>, IJournal
    {
        public ValueObjects.SkipDraft SkipDraft { get; private set; }

        public ValueObjects.CheckDate CheckDate { get; private set; }

        private Journal()
        {
        }

        public void Create(String Code, String Name, Guid ResponsibleId, Boolean CheckDate, Boolean SkipDraft)
        {
            this.SkipDraft = new ValueObjects.SkipDraft(SkipDraft);
            this.CheckDate = new ValueObjects.CheckDate(CheckDate);

            Apply<Events.Created>(e =>
            {
                e.JournalId = Id;
                e.Code = Code;
                e.Name = Name;
                e.ResponsibleId = ResponsibleId;
                e.CheckDate = CheckDate;
                e.SkipDraft = SkipDraft;
            });
        }

        public void ChangeCheckDate(Boolean Check)
        {
            this.CheckDate = new ValueObjects.CheckDate(Check);
            Apply<Events.CheckDateChanged>(e =>
            {
                e.JournalId = Id;
                e.CheckDate = Check;
            });
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.JournalId = Id;
                e.Name = Name;
            });
        }

        public void ChangeResponsible(Guid ResponsibleId)
        {
            Apply<Events.ResponsibleChanged>(e =>
            {
                e.JournalId = Id;
                e.EmployeeId = ResponsibleId;
            });
        }

        public void ChangeSkipDate(Boolean Skip)
        {
            this.SkipDraft = new ValueObjects.SkipDraft(Skip);
            Apply<Events.SkipDraftChanged>(e =>
            {
                e.JournalId = Id;
                e.SkipDraft = Skip;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.JournalId = Id;
            });
        }

        public void SetCreditAccount(Guid? AccountId)
        {
            Apply<Events.CreditAccountSet>(e =>
            {
                e.JournalId = Id;
                e.AccountId = AccountId;
            });
        }

        public void SetDebitAccount(Guid? AccountId)
        {
            Apply<Events.DebitAccountSet>(e =>
            {
                e.JournalId = Id;
                e.AccountId = AccountId;
            });
        }
    }
}