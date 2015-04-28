using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Account.Services
{
    [Api("Accounting")]
    [Route("/accounting/account/{AccountId}/destroy", "DELETE")]
    public class Destroy : IReturn<Base<Command>>
    {
        public Guid AccountId { get; set; }
    }
}