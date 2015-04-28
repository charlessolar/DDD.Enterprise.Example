using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee
{
    public class Handler :
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Destroy>,
        IHandleMessages<Commands.Hire>,
        IHandleMessages<Commands.LinkUser>,
        IHandleMessages<Commands.Terminate>,
        IHandleMessages<Commands.UpdateAddress>,
        IHandleMessages<Commands.UpdateCurrency>,
        IHandleMessages<Commands.UpdateDirectPhone>,
        IHandleMessages<Commands.UpdateEmail>,
        IHandleMessages<Commands.UpdateFax>,
        IHandleMessages<Commands.UpdateFullName>,
        IHandleMessages<Commands.UpdateGender>,
        IHandleMessages<Commands.UpdateMaritalStatus>,
        IHandleMessages<Commands.UpdateMobile>,
        IHandleMessages<Commands.UpdateNationalId>,
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
            var employee = _uow.R<Employee>().New(command.EmployeeId);
            employee.Create(command.Identity, command.FullName);
        }

        public void Handle(Commands.Destroy command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.Destroy();
        }

        public void Handle(Commands.Hire command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.Hire(command.Effective);
        }

        public void Handle(Commands.LinkUser command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            var user = _uow.R<Authentication.Users.User>().Get(command.UserId);

            employee.LinkUser(user);
        }

        public void Handle(Commands.UnlinkUser command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);

            employee.UnlinkUser();
        }

        public void Handle(Commands.Terminate command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.Terminate(command.Effective);
        }

        public void Handle(Commands.UpdateAddress command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            var country = _uow.R<Configuration.Country.Country>().Get(command.CountryId);
            employee.UpdateAddress(
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

        public void Handle(Commands.UpdateCurrency command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            var currency = _uow.R<Accounting.Currency.Currency>().Get(command.CurrencyId);
            employee.UpdateCurrency(currency);
        }

        public void Handle(Commands.UpdateDirectPhone command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateDirectPhone(command.Phone);
        }

        public void Handle(Commands.UpdateEmail command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateEmail(command.Email);
        }

        public void Handle(Commands.UpdateFax command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateFax(command.Phone);
        }

        public void Handle(Commands.UpdateFullName command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateFullName(command.FullName);
        }

        public void Handle(Commands.UpdateGender command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateGender(command.Gender);
        }

        public void Handle(Commands.UpdateMaritalStatus command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateMaritalStatus(command.MaritalStatus);
        }

        public void Handle(Commands.UpdateMobile command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateMobile(command.Phone);
        }

        public void Handle(Commands.UpdateNationalId command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateNationalId(command.NationalId);
        }

        public void Handle(Commands.UpdatePhone command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdatePhone(command.Phone);
        }

        public void Handle(Commands.UpdateWebsite command)
        {
            var employee = _uow.R<Employee>().Get(command.EmployeeId);
            employee.UpdateWebsite(command.Url);
        }
    }
}