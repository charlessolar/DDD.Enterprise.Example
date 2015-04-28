using Demo.Library.Extensions;
using NServiceBus;
using Seed.Attributes;
using Seed.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands = Demo.Domain.Relations.Store.Commands;
using Type = Seed.Types.Relations;

namespace Seed.Operations
{
    [Operation("Store")]
    [Depends("Currency", "Country")]
    [Category("Relations")]
    public class Store : IOperation
    {
        public static IEnumerable<Type.Store> Data = new[] {
            new Type.Store {
                Id = Guid.NewGuid(),
                Identity = "S001",
                Name = "Demo",
                Description = "Demo store",
                Address = new Type.Address {
                    StreetNumber = 2047,
                    StreetName = "Elk Creek",
                    StreetType = "Road",
                    City = "St. Charles",
                    PostalArea = "60174",
                    District = "Illinois",
                    Country = Country.Data.Single(x => x.Code == "US" )
                },
                Currency = Currency.Data.ElementAt(0),
                Phone = "555-555-5555",
                Fax = "",
                Email = "contact@syndeonetwork.com",
                Website = "fortissimo.syndeonetwork.com"
            },
        };

        private readonly IBus _bus;

        public Store(IBus bus)
        {
            _bus = bus;
        }

        public async Task<Boolean> Seed()
        {
            var commands = Data.Select(x => new Commands.Create
            {
                StoreId = x.Id,
                Identity = x.Identity,
                Name = x.Name,
                Timestamp = DateTime.UtcNow,
                UserId = "IMPORT"
            });
            await commands.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            var descriptions = Data.Select(x => new Commands.UpdateDescription
                {
                    StoreId = x.Id,
                    Description = x.Description,
                    Timestamp = DateTime.UtcNow,
                    UserId = "IMPORT"
                });
            await descriptions.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            var addresses = Data.Select(x => new Commands.UpdateAddress
            {
                StoreId = x.Id,
                StreetNumber = x.Address.StreetNumber,
                StreetName = x.Address.StreetName,
                StreetType = x.Address.StreetType,
                City = x.Address.City,
                PostalArea = x.Address.PostalArea,
                District = x.Address.District,
                CountryId = x.Address.Country.Id,
                Timestamp = DateTime.UtcNow,
                UserId = "IMPORT"
            });
            await addresses.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            var currencies = Data.Select(x => new Commands.UpdateCurrency
                {
                    StoreId = x.Id,
                    CurrencyId = x.Currency.Id,
                    Timestamp = DateTime.UtcNow,
                    UserId = "IMPORT"
                });
            await currencies.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            this.Done = true;
            return this.Done;
        }

        public Boolean Done { get; private set; }
    }
}