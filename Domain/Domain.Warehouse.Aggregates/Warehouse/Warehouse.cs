using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Warehouse.Warehouse
{
    public class Warehouse : Aggregates.Aggregate<Guid>, IWarehouse
    {
        private Warehouse()
        {
        }

        public void Create(String Identity, String Name)
        {
            Apply<Events.Created>(e =>
            {
                e.WarehouseId = Id;
                e.Identity = Identity;
                e.Name = Name;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.WarehouseId = Id;
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
                e.WarehouseId = Id;
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

        public void UpdateDescription(String Description)
        {
            Apply<Events.DescriptionUpdated>(e =>
            {
                e.WarehouseId = Id;
                e.Description = Description;
            });
        }

        public void UpdateManager(HumanResources.Employee.IEmployee Employee)
        {
            Apply<Events.ManagerUpdated>(e =>
            {
                e.WarehouseId = Id;
                e.EmployeeId = Employee == null ? (Guid?)null : Employee.Id;
            });
        }

        public void UpdateName(String Name)
        {
            Apply<Events.NameUpdated>(e =>
            {
                e.WarehouseId = Id;
                e.Name = Name;
            });
        }
    }
}