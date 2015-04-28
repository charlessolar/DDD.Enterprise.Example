using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Country.Services
{
    [Api("Configuration")]
    [Route("/configuration/country", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid CountryId { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
    }
}
