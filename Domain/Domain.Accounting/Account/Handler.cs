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
        IHandleMessagesAsync<Commands.ChangeDescription>,
        IHandleMessagesAsync<Commands.ChangeName>,
        IHandleMessagesAsync<Commands.ChangeType>,
        IHandleMessagesAsync<Commands.ChangeParent>,
        IHandleMessagesAsync<Commands.Freeze>,
        IHandleMessagesAsync<Commands.Unfreeze>,
        IHandleMessagesAsync<Commands.Create>,
        IHandleMessagesAsync<Commands.Destroy>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public async Task Handle(Commands.Create command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().New(command.AccountId);
            var currency = await _uow.For<Currency.Currency>().Get(command.CurrencyId);
            var store = await _uow.For<Relations.Store.Store>().Get(command.StoreId);

            account.Create(command.Code, command.Name, command.AcceptPayments, command.AllowReconcile, command.Operation, currency, store);
        }

        public async Task Handle(Commands.ChangeDescription command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.ChangeDescription(command.Description);
        }

        public async Task Handle(Commands.ChangeName command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.ChangeName(command.Name);
        }

        public async Task Handle(Commands.ChangeType command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            var accountType = await _uow.For<Configuration.AccountType.AccountType>().Get(command.TypeId);
            account.ChangeType(accountType);
        }

        public async Task Handle(Commands.ChangeParent command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            var parent = await _uow.For<Account>().Get(command.ParentId);
            account.ChangeParent(parent);
        }

        public async Task Handle(Commands.Freeze command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.Freeze();
        }

        public async Task Handle(Commands.Unfreeze command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.Unfreeze();
        }

        public async Task Handle(Commands.Destroy command, IHandleContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.Destroy();
        }
    }
}