using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal
{
    public class Handler :
        IHandleMessages<Commands.ChangeCheckDate>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangeResponsible>,
        IHandleMessages<Commands.ChangeSkipDraft>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.SetCreditAccount>,
        IHandleMessages<Commands.SetDebitAccount>
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
            var journal = _uow.R<Journal>().New(command.JournalId);
            journal.Create(command.Code, command.Name, command.ResponsibleId, command.CheckDate, command.SkipDraft);
        }

        public void Handle(Commands.ChangeCheckDate command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            journal.ChangeCheckDate(command.CheckDate);
        }

        public void Handle(Commands.ChangeName command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            journal.ChangeName(command.Name);
        }

        public void Handle(Commands.ChangeResponsible command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            journal.ChangeResponsible(command.ResponsibleId);
        }

        public void Handle(Commands.ChangeSkipDraft command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            journal.ChangeSkipDate(command.SkipDraft);
        }

        public void Handle(Commands.Destroy command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            journal.Destroy();
        }

        public void Handle(Commands.SetCreditAccount command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            journal.SetCreditAccount(command.AccountId);
        }

        public void Handle(Commands.SetDebitAccount command)
        {
            var journal = _uow.R<Journal>().Get(command.JournalId);
            journal.SetDebitAccount(command.AccountId);
        }
    }
}