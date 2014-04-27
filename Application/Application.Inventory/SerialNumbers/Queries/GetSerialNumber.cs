using Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.SerialNumbers.Queries
{
    public class GetSerialNumber : IBasicQuery
    {
        public Guid Id { get; set; }
    }
}