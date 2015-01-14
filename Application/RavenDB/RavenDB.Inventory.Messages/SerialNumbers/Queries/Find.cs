using Demo.Library.Queries;
using System;

namespace Demo.Application.RavenDB.Inventory.SerialNumbers.Queries
{
    public class Find : PagedQuery
    {
        public String Serial { get; set; }

        public DateTime? Effective { get; set; }

        public Guid? ItemId { get; set; }
    }
}