using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.Events
{
    public interface DescriptionChanged : IEvent
    {
        Guid ItemId { get; set; }

        String Description { get; set; }
    }
}