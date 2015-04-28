using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.PaymentMethod.Services
{
    [Api("Configuration")]
    [Route("/configuration/payment-method/{PaymentMethodId}", "DELETE")]
    public class Destroy : IReturn<Query<Command>>
    {
        public Guid PaymentMethodId { get; set; }
    }
}