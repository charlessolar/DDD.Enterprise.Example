using Aggregates.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee
{
    public class Employee : Aggregates.Aggregate<Guid>, IEmployee
    {
        public Aggregates.SingleValueObject<String> Identity { get; private set; }

        public Aggregates.SingleValueObject<Boolean> Employed { get; private set; }

        public Aggregates.SingleValueObject<Boolean> UserLinked { get; private set; }

        private Employee()
        {
            this.Employed = new Aggregates.SingleValueObject<bool>(false);
            this.UserLinked = new Aggregates.SingleValueObject<bool>(false);
        }

        public void Create(String Identity, String Fullname)
        {
            Apply<Events.Created>(e =>
            {
                e.EmployeeId = Id;
                e.Identity = Identity;
                e.FullName = Fullname;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.EmployeeId = Id;
            });
        }

        public void Hire(DateTime Effective)
        {
            Apply<Events.Hired>(e =>
            {
                e.EmployeeId = Id;
                e.Effective = Effective;
            });
        }

        private void Handle(Events.Hired e)
        {
            this.Employed = new Aggregates.SingleValueObject<bool>(true);
        }

        public void LinkUser(Authentication.Users.IUser User)
        {
            if (this.UserLinked.Value)
                throw new BusinessException("Already linked");

            Apply<Events.UserLinked>(e =>
            {
                e.EmployeeId = Id;
                e.UserId = User.Id;
            });
        }

        private void Handle(Events.UserLinked e)
        {
            this.UserLinked = new Aggregates.SingleValueObject<bool>(true);
        }

        public void UnlinkUser()
        {
            if (!this.UserLinked.Value)
                throw new BusinessException("Not linked");

            Apply<Events.UserUnlinked>(e =>
            {
                e.EmployeeId = Id;
            });
        }

        private void Handle(Events.UserUnlinked e)
        {
            this.UserLinked = new Aggregates.SingleValueObject<bool>(false);
        }

        public void Terminate(DateTime Effective)
        {
            Apply<Events.Terminated>(e =>
            {
                e.EmployeeId = Id;
                e.Effective = Effective;
            });
        }

        private void Handle(Events.Terminated e)
        {
            this.Employed = new Aggregates.SingleValueObject<bool>(false);
        }

        public void UpdateAddress(
            Int32 StreetNumber,
            String StreetNumberSufix,
            String StreetName,
            String StreetType,
            String StreetDirection,
            String AddressType,
            String MinorMunicipality,
            String City,
            String District,
            String PostalArea,
            Configuration.Country.ICountry Country)
        {
            Apply<Events.AddressUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.StreetNumber = StreetNumber;
                e.StreetNumberSufix = StreetNumberSufix;
                e.StreetName = StreetName;
                e.StreetType = StreetType;
                e.StreetDirection = StreetDirection;
                e.AddressType = AddressType;
                e.MinorMunicipality = MinorMunicipality;
                e.City = City;
                e.District = District;
                e.PostalArea = PostalArea;
                e.CountryId = Country.Id;
            });
        }

        public void UpdateCurrency(Accounting.Currency.ICurrency Currency)
        {
            Apply<Events.CurrencyUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.CurrencyId = Currency.Id;
            });
        }

        public void UpdateDirectPhone(String Phone)
        {
            Apply<Events.DirectPhoneUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.Phone = Phone;
            });
        }

        public void UpdateEmail(String Email)
        {
            Apply<Events.EmailUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.Email = Email;
            });
        }

        public void UpdateFax(String Phone)
        {
            Apply<Events.FaxUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.Phone = Phone;
            });
        }

        public void UpdateFullName(String FullName)
        {
            Apply<Events.FullNameUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.FullName = FullName;
            });
        }

        public void UpdateGender(GENDER Gender)
        {
            Apply<Events.GenderUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.Gender = Gender;
            });
        }

        public void UpdateMaritalStatus(MARITAL_STATUS MaritalStatus)
        {
            Apply<Events.MaritalStatusUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.MaritalStatus = MaritalStatus;
            });
        }

        public void UpdateMobile(String Phone)
        {
            Apply<Events.MobileUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.Phone = Phone;
            });
        }

        public void UpdateNationalId(String NationalId)
        {
            Apply<Events.NationalIdUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.NationalId = NationalId;
            });
        }

        public void UpdatePhone(String Phone)
        {
            Apply<Events.PhoneUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.Phone = Phone;
            });
        }

        public void UpdateWebsite(String Url)
        {
            Apply<Events.WebsiteUpdated>(e =>
            {
                e.EmployeeId = Id;
                e.Url = Url;
            });
        }
    }
}