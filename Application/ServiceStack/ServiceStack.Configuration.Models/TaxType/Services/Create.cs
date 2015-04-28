using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.TaxType.Services
{
    [Api("Configuration")]
    [Route("/configuration/tax-type", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid TaxTypeId { get; set; }

        public String Name { get; set; }
    }
}