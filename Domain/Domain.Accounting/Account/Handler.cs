using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account
{
    public class Handler :
        IHandleMessages<Commands.ChangeDescription>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangeType>,
        IHandleMessages<Commands.ChangeParent>,
        IHandleMessages<Commands.Freeze>,
        IHandleMessages<Commands.Unfreeze>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>
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
            var account = _uow.R<Account>().New(command.AccountId);
            var store = _uow.R<Relations.Store.Store>().Get(command.StoreId);
            var currency = _uow.R<Currency.Currency>().Get(command.CurrencyId);

            account.Create(command.Code, command.Name, command.AcceptPayments, command.AllowReconcile, command.Operation, store, currency);
        }

        public void Handle(Commands.ChangeDescription command)
        {
            var account = _uow.R<Account>().Get(command.AccountId);
            account.ChangeDescription(command.Description);
        }

        public void Handle(Commands.ChangeName command)
        {
            var account = _uow.R<Account>().Get(command.AccountId);
            account.ChangeName(command.Name);
        }

        public void Handle(Commands.ChangeType command)
        {
            var account = _uow.R<Account>().Get(command.AccountId);
            var accountType = _uow.R<Configuration.AccountType.AccountType>().Get(command.TypeId);
            account.ChangeType(accountType);
        }

        public void Handle(Commands.ChangeParent command)
        {
            var account = _uow.R<Account>().Get(command.AccountId);
            var parent = _uow.R<Account>().Get(command.ParentId);
            account.ChangeParent(parent);
        }

        public void Handle(Commands.Freeze command)
        {
            var account = _uow.R<Account>().Get(command.AccountId);
            account.Freeze();
        }

        public void Handle(Commands.Unfreeze command)
        {
            var account = _uow.R<Account>().Get(command.AccountId);
            account.Unfreeze();
        }

        public void Handle(Commands.Destroy command)
        {
            var account = _uow.R<Account>().Get(command.AccountId);
            account.Destroy();
        }
    }
}