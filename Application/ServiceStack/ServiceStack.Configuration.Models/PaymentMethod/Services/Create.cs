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
    [Route("/configuration/payment-method", "POST")]
    public class Create : IReturn<Query<Command>>
    {
        public Guid PaymentMethodId { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Guid? ParentId { get; set; }
    }
}