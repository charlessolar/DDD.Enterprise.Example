using System;

namespace Demo.Domain.Relations.Store
{
    public class Store : Aggregates.Aggregate<Guid>, IStore
    {
        private Store()
        {
        }

        public void Create(String Identity, String Name)
        {
            Apply<Events.Created>(e =>
            {
                e.StoreId = Id;
                e.Identity = Identity;
                e.Name = Name;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.StoreId = Id;
            });
        }

        public void AddWarehouse(Warehouse.Warehouse.IWarehouse Warehouse)
        {
            Apply<Events.WarehouseAdded>(e =>
            {
                e.StoreId = Id;
                e.WarehouseId = Warehouse.Id;
            });
        }

        public void RemoveWarehouse(Warehouse.Warehouse.IWarehouse Warehouse)
        {
            Apply<Events.WarehouseRemoved>(e =>
            {
                e.StoreId = Id;
                e.WarehouseId = Warehouse.Id;
            });
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
                e.StoreId = Id;
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
                e.StoreId = Id;
                e.CurrencyId = Currency.Id;
            });
        }

        public void UpdateName(String Name)
        {
            Apply<Events.NameUpdated>(e =>
            {
                e.StoreId = Id;
                e.Name = Name;
            });
        }

        public void UpdateDescription(String Description)
        {
            Apply<Events.DescriptionUpdated>(e =>
            {
                e.StoreId = Id;
                e.Description = Description;
            });
        }

        public void UpdateEmail(String Email)
        {
            Apply<Events.EmailUpdated>(e =>
            {
                e.StoreId = Id;
                e.Email = Email;
            });
        }

        public void UpdateFax(String Phone)
        {
            Apply<Events.FaxUpdated>(e =>
            {
                e.StoreId = Id;
                e.Phone = Phone;
            });
        }

        public void UpdatePhone(String Phone)
        {
            Apply<Events.PhoneUpdated>(e =>
            {
                e.StoreId = Id;
                e.Phone = Phone;
            });
        }

        public void UpdateWebsite(String Url)
        {
            Apply<Events.WebsiteUpdated>(e =>
            {
                e.StoreId = Id;
                e.Url = Url;
            });
        }
    }
}