using Aggregates;
using NServiceBus;

namespace Demo.Domain.Relations.Store
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.AddWarehouse>,
        IHandleMessages<Commands.RemoveWarehouse>,
        IHandleMessages<Commands.UpdateName>,
        IHandleMessages<Commands.UpdateDescription>,
        IHandleMessages<Commands.UpdateCurrency>,
        IHandleMessages<Commands.UpdateAddress>,
        IHandleMessages<Commands.UpdateEmail>,
        IHandleMessages<Commands.UpdateFax>,
        IHandleMessages<Commands.UpdatePhone>,
        IHandleMessages<Commands.UpdateWebsite>
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
            var store = _uow.R<Store>().New(command.StoreId);
            store.Create(command.Identity, command.Name);
        }

        public void Handle(Commands.Destroy command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            store.Destroy();
        }

        public void Handle(Commands.AddWarehouse command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            var warehouse = _uow.R<Warehouse.Warehouse.Warehouse>().Get(command.WarehouseId);
            store.AddWarehouse(warehouse);
        }

        public void Handle(Commands.RemoveWarehouse command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            var warehouse = _uow.R<Warehouse.Warehouse.Warehouse>().Get(command.WarehouseId);
            store.RemoveWarehouse(warehouse);
        }

        public void Handle(Commands.UpdateName command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            store.UpdateName(command.Name);
        }

        public void Handle(Commands.UpdateDescription command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            store.UpdateDescription(command.Description);
        }

        public void Handle(Commands.UpdateCurrency command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            var currency = _uow.R<Domain.Accounting.Currency.Currency>().Get(command.CurrencyId);
            store.UpdateCurrency(currency);
        }

        public void Handle(Commands.UpdateAddress command)
        {
            var country = _uow.R<Domain.Configuration.Country.Country>().Get(command.CountryId);
            var store = _uow.R<Store>().Get(command.StoreId);
            store.UpdateAddress(
                command.StreetNumber,
                command.StreetNumberSufix,
                command.StreetName,
                command.StreetType,
                command.StreetDirection,
                command.AddressType,
                command.MinorMunicipality,
                command.City,
                command.District,
                command.PostalArea,
                country
                );
        }

        public void Handle(Commands.UpdateEmail command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            store.UpdateEmail(command.Email);
        }

        public void Handle(Commands.UpdateFax command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            store.UpdateFax(command.Phone);
        }

        public void Handle(Commands.UpdatePhone command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            store.UpdatePhone(command.Phone);
        }

        public void Handle(Commands.UpdateWebsite command)
        {
            var store = _uow.R<Store>().Get(command.StoreId);
            store.UpdateWebsite(command.Url);
        }
    }
}