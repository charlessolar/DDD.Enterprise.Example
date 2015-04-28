using Demo.Library.Responses;
using ServiceStack.Model;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public DateTime Effective { get; set; }

        public String Reference { get; set; }

        public String Account { get; set; }

        public String Journal { get; set; }

        public String Period { get; set; }

        public Decimal Amount { get; set; }

        public Boolean Reconciled { get; set; }

        public Decimal ReconciledAmount { get; set; }
    }
}