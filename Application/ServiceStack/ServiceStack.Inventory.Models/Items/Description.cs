using Forte.Library.Responses;
using ServiceStack;
using System;

namespace Forte.Application.ServiceStack.Inventory.Models.Items
{
    [Api("Inventory")]
    [Route("/items/{ItemId}/description", "POST")]
    public class Description : IReturn<DescriptionResponse>
    {
        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid ItemId { get; set; }

        [ApiMember(IsRequired = true)]
        public String Data { get; set; }
    }

    public class DescriptionResponse : Base
    {
    }
}