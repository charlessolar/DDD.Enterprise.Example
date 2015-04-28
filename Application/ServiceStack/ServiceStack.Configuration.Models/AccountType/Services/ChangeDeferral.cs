using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.AccountType.Services
{
    [Api("Configuration")]
    [Route("/configuration/account-type/{AccountTypeId}/deferral", "PUT POST")]
    public class ChangeDeferral : IReturn<Base<Command>>
    {
        public Guid AccountTypeId { get; set; }

        public String DeferralMethod { get; set; }
    }
}