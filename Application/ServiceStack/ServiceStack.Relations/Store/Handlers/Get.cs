using Demo.Domain.Relations.Store.Events;
using Demo.Library.Extensions;
using Nest;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Handlers
{
    public class Get :
        IHandleMessages<AddressUpdated>,
        IHandleMessages<Created>,
        IHandleMessages<CurrencyUpdated>,
        IHandleMessages<NameUpdated>,
        IHandleMessages<DescriptionUpdated>,
        IHandleMessages<Destroyed>,
        IHandleMessages<EmailUpdated>,
        IHandleMessages<FaxUpdated>,
        IHandleMessages<PhoneUpdated>,
        IHandleMessages<WebsiteUpdated>
    {
        private readonly IDocumentStore _store;
        private readonly IElasticClient _elastic;

        public Get(IDocumentStore store, IElasticClient elastic)
        {
            _store = store;
            _elastic = elastic;
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var get = new Responses.Get
                {
                    Id = e.StoreId,
                    Name = e.Name,
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
                var get = session.Load<Responses.Get>(e.StoreId);

                session.Delete(get);
                session.SaveChanges();
            }
        }

        public void Handle(AddressUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);

                var country = _elastic.Get<Configuration.Country.Responses.Index>(e.CountryId);

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
                    Country = country.Source.Name
                };

                session.SaveChanges();
            }
        }

        public void Handle(CurrencyUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);
                var currency = _elastic.Get<Accounting.Currency.Responses.Index>(e.CurrencyId);

                get.Currency = currency.Source.Name;
                get.CurrencyId = currency.Source.Id;

                session.SaveChanges();
            }
        }

        public void Handle(NameUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);
                get.Name = e.Name;

                session.SaveChanges();
            }
        }

        public void Handle(DescriptionUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);
                get.Description = e.Description;

                session.SaveChanges();
            }
        }

        public void Handle(EmailUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);
                get.Email = e.Email;

                session.SaveChanges();
            }
        }

        public void Handle(FaxUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);
                get.Fax = e.Phone;

                session.SaveChanges();
            }
        }

        public void Handle(PhoneUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);
                get.Phone = e.Phone;

                session.SaveChanges();
            }
        }

        public void Handle(WebsiteUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.StoreId);
                get.Website = e.Url;

                session.SaveChanges();
            }
        }
    }
}