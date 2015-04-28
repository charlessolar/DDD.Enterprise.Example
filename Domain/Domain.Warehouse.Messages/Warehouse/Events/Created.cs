using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Warehouse.Warehouse.Events
{
    public interface Created : IEvent
    {
        Guid WarehouseId { get; set; }

        String Identity { get; set; }

        String Name { get; set; }
    }
}