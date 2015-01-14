using Demo.Library.Queries;
using System;

namespace Demo.Application.RavenDB.Inventory.Items.Queries
{
    public class Find : PagedQuery
    {
        public String Number { get; set; }

        public String Description { get; set; }
    }
}