using Demo.Library.Queries;
using System;

namespace Demo.Application.RavenDB.Inventory.Items.Queries
{
    public class Get : BasicQuery
    {
        public Guid Id { get; set; }
    }
}