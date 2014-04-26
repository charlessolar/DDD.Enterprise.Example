using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.Items.Commands
{
    public class ChangeDescription
    {
        public Guid ItemId { get; set; }

        public String Description { get; set; }
    }
}