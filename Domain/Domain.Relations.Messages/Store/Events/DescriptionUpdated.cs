using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Relations.Store.Events
{
    public interface DescriptionUpdated : IEvent
    {
        Guid StoreId { get; set; }

        String Description { get; set; }
    }
}