using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Commands
{
    public class AddWarehouse : DemoCommand
    {
        public Guid StoreId { get; set; }

        public Guid WarehouseId { get; set; }
    }
}