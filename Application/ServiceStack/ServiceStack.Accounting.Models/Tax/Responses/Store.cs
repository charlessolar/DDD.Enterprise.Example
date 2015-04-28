using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Tax.Responses
{
    public class Store : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public Guid StoreId { get; set; }

        public Guid TaxId { get; set; }

        public String Code { get; set; }
    }
}