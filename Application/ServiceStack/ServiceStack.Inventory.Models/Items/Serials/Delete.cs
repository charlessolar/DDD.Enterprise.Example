using Forte.Library.Responses;
using ServiceStack;
using System;

namespace Forte.Application.ServiceStack.Inventory.Models.Items.Serials
{
    [Route("/items/{ItemId}/serials/{Id}", "DELETE")]
    public class Delete : IReturn<Base>
    {
        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid ItemId { get; set; }

        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid Id { get; set; }
    }
}