using Forte.Library.Queries;
using Forte.Library.Responses;
using ServiceStack;
using ServiceStack.Model;
using System;

namespace Forte.Application.ServiceStack.Inventory.Models.Items.Serials
{
    [Route("/items/{ItemId}/serials/{Id}", "GET")]
    public class Get : BasicQuery, IReturn<GetResponse>
    {
        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid ItemId { get; set; }

        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid Id { get; set; }
    }

    public class GetResponse : Base, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Serial { get; set; }

        public Decimal Quantity { get; set; }

        public DateTime Effective { get; set; }
    }
}