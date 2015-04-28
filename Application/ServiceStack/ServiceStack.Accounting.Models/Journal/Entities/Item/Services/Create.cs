using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/item", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid ItemId { get; set; }

        public DateTime Effective { get; set; }

        public String Reference { get; set; }

        public Guid AccountId { get; set; }

        public Guid PeriodId { get; set; }

        public Decimal Amount { get; set; }
    }
}