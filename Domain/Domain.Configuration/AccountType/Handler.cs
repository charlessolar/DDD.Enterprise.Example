using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType
{
    public class Handler :
        IHandleMessages<Commands.ChangeDeferral>,
        IHandleMessages<Commands.ChangeDescription>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>
    {
        private readonly IUnitOfWork _uow;
        

        public Handler(IUnitOfWork uow)
        {
            _uow = uow;
           
        }

        public async Task Handle(Commands.ChangeDeferral command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<AccountType>().Get(command.AccountTypeId);
            account.ChangeDeferral(command.DeferralMethod);
        }

        public async Task Handle(Commands.ChangeDescription command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<AccountType>().Get(command.AccountTypeId);
            account.ChangeDescription(command.Description);
        }

        public async Task Handle(Commands.ChangeName command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<AccountType>().Get(command.AccountTypeId);
            account.ChangeName(command.Name);
        }

        public async Task Handle(Commands.Create command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<AccountType>().New(command.AccountTypeId);
            account.Create(command.Name, command.DeferralMethod, command.ParentId);
        }

        public async Task Handle(Commands.Destroy command, IMessageHandlerContext ctx)
        {
            var account = await _uow.For<AccountType>().Get(command.AccountTypeId);
            account.Destroy();
        }
    }
}