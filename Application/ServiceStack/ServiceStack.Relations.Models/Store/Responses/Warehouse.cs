using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Responses
{
    public class Warehouse : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public Guid WarehouseId { get; set; }

        public String Identity { get; set; }
    }
}