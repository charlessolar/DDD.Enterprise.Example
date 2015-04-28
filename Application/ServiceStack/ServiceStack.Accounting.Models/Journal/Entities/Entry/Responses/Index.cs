using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public Boolean Open { get; set; }

        public Boolean NeedsReview { get; set; }

        public String ReviewEmployee { get; set; }

        public STATE State { get; set; }

        public Decimal Debits { get; set; }

        public Decimal Credits { get; set; }

        public ICollection<Guid> Items { get; set; }
    }
}