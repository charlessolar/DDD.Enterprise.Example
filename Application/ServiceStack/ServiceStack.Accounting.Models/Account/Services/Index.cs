using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace Demo.Application.ServiceStack.Accounting.Account.Services
{
    [Api("Accounting")]
    [Route("/accounting/account", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
    }
}