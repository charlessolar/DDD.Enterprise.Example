using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Services
{
    [Api("Relations")]
    [Route("/relations/store", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid StoreId { get; set; }

        public String Identity { get; set; }

        public String Name { get; set; }
    }
}