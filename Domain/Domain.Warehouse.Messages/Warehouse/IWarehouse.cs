using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Warehouse.Warehouse
{
    public interface IWarehouse : Aggregates.Contracts.IEventSource<Guid>
    {
    }
}