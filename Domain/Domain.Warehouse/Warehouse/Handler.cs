using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Warehouse.Warehouse
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.UpdateAddress>,
        IHandleMessages<Commands.UpdateDescription>,
        IHandleMessages<Commands.UpdateManager>,
        IHandleMessages<Commands.UpdateName>
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
            var warehouse = _uow.R<Warehouse>().New(command.WarehouseId);
            warehouse.Create(command.Identity, command.Name);
        }

        public void Handle(Commands.Destroy command)
        {
            var warehouse = _uow.R<Warehouse>().Get(command.WarehouseId);
            warehouse.Destroy();
        }

        public void Handle(Commands.UpdateAddress command)
        {
            var warehouse = _uow.R<Warehouse>().Get(command.WarehouseId);
            var country = _uow.R<Configuration.Country.Country>().Get(command.CountryId);
            warehouse.UpdateAddress(
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
                country);
        }

        public void Handle(Commands.UpdateDescription command)
        {
            var warehouse = _uow.R<Warehouse>().Get(command.WarehouseId);
            warehouse.UpdateDescription(command.Description);
        }

        public void Handle(Commands.UpdateManager command)
        {
            var warehouse = _uow.R<Warehouse>().Get(command.WarehouseId);
            if (command.EmployeeId.HasValue)
            {
                var employee = _uow.R<HumanResources.Employee.Employee>().Get(command.EmployeeId);

                warehouse.UpdateManager(employee);
            }
            else
                warehouse.UpdateManager(null);
        }

        public void Handle(Commands.UpdateName command)
        {
            var warehouse = _uow.R<Warehouse>().Get(command.WarehouseId);
            warehouse.UpdateName(command.Name);
        }
    }
}