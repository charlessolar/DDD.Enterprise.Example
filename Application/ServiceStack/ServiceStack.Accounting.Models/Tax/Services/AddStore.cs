using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/store", "POST")]
    public class AddStore : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public Guid StoreId { get; set; }
    }
}