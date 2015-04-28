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
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.ChangeDeferral command)
        {
            var account = _uow.R<AccountType>().Get(command.AccountTypeId);
            account.ChangeDeferral(command.DeferralMethod);
        }

        public void Handle(Commands.ChangeDescription command)
        {
            var account = _uow.R<AccountType>().Get(command.AccountTypeId);
            account.ChangeDescription(command.Description);
        }

        public void Handle(Commands.ChangeName command)
        {
            var account = _uow.R<AccountType>().Get(command.AccountTypeId);
            account.ChangeName(command.Name);
        }

        public void Handle(Commands.Create command)
        {
            var account = _uow.R<AccountType>().New(command.AccountTypeId);
            account.Create(command.Name, command.DeferralMethod, command.ParentId);
        }

        public void Handle(Commands.Destroy command)
        {
            var account = _uow.R<AccountType>().Get(command.AccountTypeId);
            account.Destroy();
        }
    }
}