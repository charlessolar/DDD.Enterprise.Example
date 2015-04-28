using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
        public Guid? Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Boolean? Activated { get; set; }
    }
}