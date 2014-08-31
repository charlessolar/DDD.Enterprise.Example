using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Models.Items.Services
{
    [Route("/items", "POST")]
    public class CreateItem : IReturn<IdResponse>
    {
        public Guid Id { get; set; }

        public String Number { get; set; }
        public String Description { get; set; }

        public String UnitOfMeasure { get; set; }

        public Decimal? CatalogPrice { get; set; }
        public Decimal? CostPrice { get; set; }
    }
}