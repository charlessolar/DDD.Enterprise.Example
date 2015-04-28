using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Services
{
    [Api("Relations")]
    [Route("/relations/store/{StoreId}/currency", "PUT POST")]
    public class UpdateCurrency : IReturn<Base<Command>>
    {
        public Guid StoreId { get; set; }

        public Guid CurrencyId { get; set; }
    }
}