using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Account.Services
{
    [Api("Accounting")]
    [Route("/accounting/account/{AccountId}/type", "PUT POST")]
    public class ChangeType : IReturn<Base<Command>>
    {
        public Guid AccountId { get; set; }

        public Guid TypeId { get; set; }
    }
}