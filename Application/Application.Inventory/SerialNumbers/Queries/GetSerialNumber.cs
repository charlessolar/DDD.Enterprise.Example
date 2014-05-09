using Demo.Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.SerialNumbers.Queries
{
    public class GetSerialNumber : BasicQuery
    {
        public Guid Id { get; set; }
    }
}