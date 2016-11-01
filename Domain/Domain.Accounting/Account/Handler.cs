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
        

        public Handler(IUnitOfWork uow)
        {
            _uow = uow;
           
        }

        public async Task Handle(Commands.Create command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().New(command.AccountId);
            var currency = await _uow.For<Currency.Currency>().Get(command.CurrencyId);

            account.Create(command.Code, command.Name, command.AcceptPayments, command.AllowReconcile, command.Operation, currency);
        }

        public async Task Handle(Commands.ChangeDescription command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.ChangeDescription(command.Description);
        }

        public async Task Handle(Commands.ChangeName command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.ChangeName(command.Name);
        }

        public async Task Handle(Commands.ChangeType command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            var accountType = await _uow.For<Configuration.AccountType.AccountType>().Get(command.TypeId);
            account.ChangeType(accountType);
        }

        public async Task Handle(Commands.ChangeParent command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            var parent = await _uow.For<Account>().Get(command.ParentId);
            account.ChangeParent(parent);
        }

        public async Task Handle(Commands.Freeze command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.Freeze();
        }

        public async Task Handle(Commands.Unfreeze command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.Unfreeze();
        }

        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<Account>().Get(command.AccountId);
            account.Destroy();
        }
    }
}