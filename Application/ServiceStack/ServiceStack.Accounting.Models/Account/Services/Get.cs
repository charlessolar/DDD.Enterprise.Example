using Demo.Library.Queries;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Account.Services
{
    [Api("Accounting")]
    [Route("/accounting/account/{AccountId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid AccountId { get; set; }
    }
}