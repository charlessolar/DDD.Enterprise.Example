using Forte.Library.Queries;
using Forte.Library.Responses;
using ServiceStack;
using ServiceStack.Model;
using System;

namespace Forte.Application.ServiceStack.Inventory.Models.Items
{
    [Api("Inventory")]
    [Route("/items/{Id}", "GET")]
    public class Get : BasicQuery, IReturn<GetResponse>
    {
        [ApiMember(ParameterType = "path", IsRequired = true, DataType = "guid")]
        public Guid Id { get; set; }
    }

    public class GetResponse : Base, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Number { get; set; }

        public String Description { get; set; }

        public String UnitOfMeasure { get; set; }

        public Decimal? CatalogPrice { get; set; }

        public Decimal? CostPrice { get; set; }
    }
}