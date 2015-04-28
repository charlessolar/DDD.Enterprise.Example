using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Services
{
    [Api("Relations")]
    [Route("/relations/store/{StoreId}/warehouse", "PUT POST")]
    public class AddWarehouse : IReturn<Base<Command>>
    {
        public Guid StoreId { get; set; }

        public Guid WarehouseId { get; set; }
    }
}