using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.Commands
{
    public class ChangeDescription : ICommand
    {
        public Guid ItemId { get; set; }

        public String Description { get; set; }
    }
}