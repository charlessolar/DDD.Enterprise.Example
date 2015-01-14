using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Presentation.Inventory.Models.Items.Serials
{
    [Route("/items/{ItemId}/serials", "POST")]
    public class Create : IReturn<Base>
    {
        [ApiMember(ParameterType = "path", IsRequired = true)]
        public Guid ItemId { get; set; }

        [ApiMember(IsRequired = true)]
        public Guid SerialNumberId { get; set; }

        [ApiMember(IsRequired = true)]
        public String SerialNumber { get; set; }

        [ApiMember(IsRequired = true)]
        public Decimal Quantity { get; set; }

        [ApiMember(IsRequired = true)]
        public DateTime Effective { get; set; }
    }
}