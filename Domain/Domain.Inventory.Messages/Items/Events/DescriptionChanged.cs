using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.Events
{
    public interface DescriptionChanged : IEvent
    {
        Guid ItemId { get; set; }

        String Description { get; set; }
    }
}