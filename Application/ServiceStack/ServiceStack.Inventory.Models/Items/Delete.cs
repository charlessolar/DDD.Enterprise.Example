using Forte.Library.Responses;
using ServiceStack;
using System;

namespace Forte.Application.ServiceStack.Inventory.Models.Items
{
    [Api("Inventory")]
    [Route("/items/{Id}", "DELETE")]
    public class Delete : IReturn<Command>
    {
        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid Id { get; set; }
    }
}