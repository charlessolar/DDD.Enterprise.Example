using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.AccountType.Services
{
    [Api("Configuration")]
    [Route("/configuration/account-type/{AccountTypeId}", "GET")]
    public class Get : Query<Responses.Index>
    {
        public Guid AccountTypeId { get; set; }
    }
}