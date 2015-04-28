using Demo.Domain.HumanResources.Employee.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Handlers
{
    public class Get :
        IHandleMessages<AddressUpdated>,
        IHandleMessages<Created>,
        IHandleMessages<CurrencyUpdated>,
        IHandleMessages<Destroyed>,
        IHandleMessages<DirectPhoneUpdated>,
        IHandleMessages<EmailUpdated>,
        IHandleMessages<FaxUpdated>,
        IHandleMessages<FullNameUpdated>,
        IHandleMessages<GenderUpdated>,
        IHandleMessages<Hired>,
        IHandleMessages<MaritalStatusUpdated>,
        IHandleMessages<MobileUpdated>,
        IHandleMessages<NationalIdUpdated>,
        IHandleMessages<PhoneUpdated>,
        IHandleMessages<Terminated>,
        IHandleMessages<UserLinked>,
        IHandleMessages<WebsiteUpdated>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var get = new Responses.Get
                {
                    Id = e.EmployeeId,
                    FullName = e.FullName,
                    Identity = e.Identity,
                };
                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);

                session.Delete(get);
                session.SaveChanges();
            }
        }

        public void Handle(AddressUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                var country = session.Load<Configuration.Country.Responses.Index>(e.CountryId);
                get.Address = new Responses.Address
                {
                    StreetNumber = e.StreetNumber,
                    StreetNumberSufix = e.StreetNumberSufix,
                    StreetName = e.StreetName,
                    StreetType = e.StreetType,
                    StreetDirection = e.StreetDirection,
                    AddressType = e.AddressType,
                    AddressTypeId = e.AddressTypeId,
                    MinorMunicipality = e.MinorMunicipality,
                    City = e.City,
                    District = e.District,
                    PostalArea = e.PostalArea,
                    Country = country.Name
                };

                session.SaveChanges();
            }
        }

        public void Handle(CurrencyUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                var currency = session.Load<Accounting.Currency.Responses.Index>(e.CurrencyId);
                get.Currency = currency.Name;
                get.CurrencyId = currency.Id;

                session.SaveChanges();
            }
        }

        public void Handle(DirectPhoneUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.DirectPhone = e.Phone;

                session.SaveChanges();
            }
        }

        public void Handle(EmailUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Email = e.Email;

                session.SaveChanges();
            }
        }

        public void Handle(FaxUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Fax = e.Phone;

                session.SaveChanges();
            }
        }

        public void Handle(FullNameUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.FullName = e.FullName;

                session.SaveChanges();
            }
        }

        public void Handle(GenderUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Gender = e.Gender.DisplayName;

                session.SaveChanges();
            }
        }

        public void Handle(Hired e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Hired = e.Effective;

                session.SaveChanges();
            }
        }

        public void Handle(MaritalStatusUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.MaritalStatus = e.MaritalStatus.DisplayName;

                session.SaveChanges();
            }
        }

        public void Handle(MobileUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Mobile = e.Phone;

                session.SaveChanges();
            }
        }

        public void Handle(NationalIdUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.NationalId = e.NationalId;

                session.SaveChanges();
            }
        }

        public void Handle(PhoneUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Phone = e.Phone;

                session.SaveChanges();
            }
        }

        public void Handle(Terminated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Terminated = e.Effective;

                session.SaveChanges();
            }
        }

        public void Handle(UserLinked e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                var user = session.Load<Authentication.Users.Responses.Get>(e.UserId);
                get.UserId = user.Id;

                session.SaveChanges();
            }
        }

        public void Handle(UserUnlinked e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.UserId = "";

                session.SaveChanges();
            }
        }

        public void Handle(WebsiteUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.EmployeeId);
                get.Website = e.Url;

                session.SaveChanges();
            }
        }
    }
}