using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Tax.Responses
{
    public class Region : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public Guid TaxId { get; set; }

        public Guid RegionId { get; set; }

        public String Code { get; set; }
    }
}