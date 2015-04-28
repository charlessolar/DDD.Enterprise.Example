using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax
{
    public class Handler :
        IHandleMessages<Commands.Activate>,
        IHandleMessages<Commands.AddRegion>,
        IHandleMessages<Commands.AddStore>,
        IHandleMessages<Commands.ChangeAccount>,
        IHandleMessages<Commands.ChangeDescription>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangeRate>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Deactivate>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.RemoveRegion>,
        IHandleMessages<Commands.RemoveStore>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Activate command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            var user = _uow.R<Authentication.Users.User>().Get(command.UserId);
            tax.Activate(user);
        }

        public void Handle(Commands.AddRegion command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            var region = _uow.R<Configuration.Region.Region>().Get(command.RegionId);
            tax.AddRegion(region);
        }

        public void Handle(Commands.AddStore command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            var store = _uow.R<Relations.Store.Store>().Get(command.StoreId);
            tax.AddStore(store);
        }

        public void Handle(Commands.ChangeAccount command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            var account = _uow.R<Account.Account>().Get(command.AccountId);
            tax.ChangeAccount(account);
        }

        public void Handle(Commands.ChangeDescription command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            tax.ChangeDescription(command.Description);
        }

        public void Handle(Commands.ChangeName command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            tax.ChangeName(command.Name);
        }

        public void Handle(Commands.ChangeRate command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            tax.ChangeRate(command.Fixed, command.Rate);
        }

        public void Handle(Commands.Create command)
        {
            var tax = _uow.R<Tax>().New(command.TaxId);
            var taxType = _uow.R<Configuration.TaxType.TaxType>().Get(command.TaxTypeId);

            tax.Create(command.Code, command.Name, command.Description, taxType);
        }

        public void Handle(Commands.Deactivate command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            var user = _uow.R<Authentication.Users.User>().Get(command.UserId);
            tax.Deactivate(user);
        }

        public void Handle(Commands.Destroy command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            tax.Destroy();
        }

        public void Handle(Commands.RemoveRegion command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            var region = _uow.R<Configuration.Region.Region>().Get(command.RegionId);
            tax.RemoveRegion(region);
        }

        public void Handle(Commands.RemoveStore command)
        {
            var tax = _uow.R<Tax>().Get(command.TaxId);
            var store = _uow.R<Relations.Store.Store>().Get(command.StoreId);
            tax.RemoveStore(store);
        }
    }
}