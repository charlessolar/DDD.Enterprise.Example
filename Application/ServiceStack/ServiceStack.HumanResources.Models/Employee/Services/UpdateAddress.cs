using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Services
{
    [Api("HumanResources")]
    [Route("/human-resources/employee/{EmployeeId}/address", "PUT POST")]
    public class UpdateAddress : IReturn<Base<Command>>
    {
        public Guid EmployeeId { get; set; }

        public Int32 StreetNumber { get; set; }

        public String StreetNumberSufix { get; set; }

        public String StreetName { get; set; }

        public String StreetType { get; set; }

        public String StreetDirection { get; set; }

        public String AddressType { get; set; }

        public String AddressTypeId { get; set; }

        public String MinorMunicipality { get; set; }

        public String City { get; set; }

        public String District { get; set; }

        public String PostalArea { get; set; }

        public Guid CountryId { get; set; }
    }
}