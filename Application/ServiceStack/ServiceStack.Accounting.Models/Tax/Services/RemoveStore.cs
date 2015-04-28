using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/store", "DELETE")]
    public class RemoveStore : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public Guid StoreId { get; set; }
    }
}