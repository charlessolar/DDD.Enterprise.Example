using Forte.Library.Queries;
using Forte.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace Forte.Application.ServiceStack.Inventory.Models.Items.Serials
{
    [Route("/items/{ItemId}/serials", "GET")]
    public class Find : PagedQuery, IReturn<FindResponse>
    {
        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid ItemId { get; set; }

        public String Serial { get; set; }

        [ApiMember(DataType = "datetime")]
        public DateTime? Effective { get; set; }
    }

    public class FindResponse : Base, IIsList<GetResponse>
    {
        public IEnumerable<GetResponse> Results { get; set; }
    }
}