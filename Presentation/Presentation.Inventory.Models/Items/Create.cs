using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Presentation.Inventory.Models.Items
{
    [Api("Inventory")]
    [Route("/items", "POST")]
    public class Create : IReturn<Command>
    {
        [ApiMember(IsRequired = true, DataType = "guid")]
        public Guid ItemId { get; set; }

        [ApiMember(IsRequired = true)]
        public String Number { get; set; }

        [ApiMember(IsRequired = true)]
        public String Description { get; set; }

        [ApiMember(IsRequired = true)]
        public String UnitOfMeasure { get; set; }

        public Decimal? CatalogPrice { get; set; }

        public Decimal? CostPrice { get; set; }
    }
}