using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Services
{
    [Api("Relations")]
    [Route("/relations/store/{StoreId}/description", "PUT POST")]
    public class UpdateDescription : IReturn<Base<Command>>
    {
        public Guid StoreId { get; set; }

        public String Description { get; set; }
    }
}