using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Account.Services
{
    [Api("Accounting")]
    [Route("/accounting/account/{AccountId}/name", "PUT POST")]
    public class ChangeName : IReturn<Base<Command>>
    {
        public Guid AccountId { get; set; }

        public String Name { get; set; }
    }
}