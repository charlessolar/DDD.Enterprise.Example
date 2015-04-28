using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.PaymentMethod.Entities.Detail.Services
{
    [Api("Configuration")]
    [Route("/configuration/payment-method/{PaymentMethodId}/detail", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
        public Guid PaymentMethodId { get; set; }
    }
}