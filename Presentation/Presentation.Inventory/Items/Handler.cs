using Demo.Domain.Inventory.Items.Events;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items
{
    public class Handler : IHandleMessages<Created>, IHandleMessages<DescriptionChanged>
    {
        public void Handle(Created e)
        {
        }
        public void Handle(DescriptionChanged e)
        {
        }
    }
}
