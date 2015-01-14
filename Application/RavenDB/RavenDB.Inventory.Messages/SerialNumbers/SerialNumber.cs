using System;

namespace Demo.Application.RavenDB.Inventory.SerialNumbers
{
    public class SerialNumber
    {
        public Guid Id { get; set; }

        public String Serial { get; set; }

        public Decimal Quantity { get; set; }

        public DateTime Effective { get; set; }

        public Guid ItemId { get; set; }
    }
}