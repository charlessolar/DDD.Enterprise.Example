using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.ValueObjects
{
    public class Description : ValueObject<Description>
    {
        public readonly String Text;

        public Description(String Description)
        {
            this.Text = Description;
        }
    }
}
