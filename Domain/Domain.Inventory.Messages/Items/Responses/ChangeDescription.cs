using System;

namespace Demo.Domain.Inventory.Items.Responses
{
    public class ChangeDescription
    {
        public Guid ItemId { get; set; }

        public String Description { get; set; }
    }
}