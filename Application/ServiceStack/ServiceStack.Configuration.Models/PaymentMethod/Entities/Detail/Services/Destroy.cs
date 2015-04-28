using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.PaymentMethod.Entities.Detail.Services
{
    [Api("Configuration")]
    [Route("/configuration/payment-method/{PaymentMethodId}/detail/{DetailId}", "DELETE")]
    public class Destroy : IReturn<Base<Command>>
    {
        public Guid PaymentMethodId { get; set; }

        public Guid DetailId { get; set; }
    }
}