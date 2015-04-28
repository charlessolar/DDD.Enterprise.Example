using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Events
{
    public interface AddressUpdated : IEvent
    {
        Guid EmployeeId { get; set; }

        Int32 StreetNumber { get; set; }

        String StreetNumberSufix { get; set; }

        String StreetName { get; set; }

        String StreetType { get; set; }

        String StreetDirection { get; set; }

        String AddressType { get; set; }

        String AddressTypeId { get; set; }

        String MinorMunicipality { get; set; }

        String City { get; set; }

        String District { get; set; }

        String PostalArea { get; set; }

        Guid CountryId { get; set; }
    }
}