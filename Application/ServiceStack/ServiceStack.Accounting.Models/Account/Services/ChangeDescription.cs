using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Account.Services
{
    [Api("Accounting")]
    [Route("/accounting/account/{AccountId}/description", "PUT POST")]
    public class ChangeDescription : IReturn<Base<Command>>
    {
        public Guid AccountId { get; set; }

        public String Description { get; set; }
    }
}