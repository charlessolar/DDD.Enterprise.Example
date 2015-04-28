using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Services
{
    [Api("Relations")]
    [Route("/relations/store/{StoreId}", "DELETE")]
    public class Destroy : IReturn<Base<Command>>
    {
        public Guid StoreId { get; set; }
    }
}