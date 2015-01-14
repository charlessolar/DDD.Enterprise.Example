using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.Commands
{
    public class ChangeDescription : ICommand
    {
        public Guid ItemId { get; set; }

        public String Description { get; set; }
    }
}