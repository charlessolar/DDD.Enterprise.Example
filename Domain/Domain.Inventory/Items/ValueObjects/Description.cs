using Aggregates;
using System;

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