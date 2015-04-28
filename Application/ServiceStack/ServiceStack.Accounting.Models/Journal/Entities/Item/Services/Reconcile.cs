using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/item/{ItemId}/reconcile", "POST")]
    public class Reconcile : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid ItemId { get; set; }

        public Guid OtherItemId { get; set; }

        public Decimal Amount { get; set; }
    }
}