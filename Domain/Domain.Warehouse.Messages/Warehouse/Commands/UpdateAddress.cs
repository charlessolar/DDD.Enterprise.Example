using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Warehouse.Warehouse.Commands
{
    public class UpdateAddress : DemoCommand
    {
        public Guid WarehouseId { get; set; }

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