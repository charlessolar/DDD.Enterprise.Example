using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Commands
{
    public class RemoveWarehouse : DemoCommand
    {
        public Guid StoreId { get; set; }

        public Guid WarehouseId { get; set; }
    }
}