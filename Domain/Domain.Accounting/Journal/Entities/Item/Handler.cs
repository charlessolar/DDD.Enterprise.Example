using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item
{
    public class Handler :
        IHandleMessages<Commands.ChangeEffective>,
        IHandleMessages<Commands.ChangeReference>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.Reconcile>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Create command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            var item = journal.Entity<Item>().New(command.ItemId);

            item.Create(command.Effective, command.Reference, command.AccountId, command.PeriodId, command.Amount);
        }

        public void Handle(Commands.ChangeEffective command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            var item = journal.Entity<Item>().Get(command.ItemId);

            item.ChangeEffective(command.Effective);
        }

        public void Handle(Commands.ChangeReference command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            var item = journal.Entity<Item>().Get(command.ItemId);

            item.ChangeReference(command.Reference);
        }

        public void Handle(Commands.Destroy command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            var item = journal.Entity<Item>().Get(command.ItemId);

            item.Destroy();
        }

        public void Handle(Commands.Reconcile command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            var item = journal.Entity<Item>().Get(command.ItemId);

            item.Reconcile(command.OtherItemId, command.Amount);
        }
    }
}