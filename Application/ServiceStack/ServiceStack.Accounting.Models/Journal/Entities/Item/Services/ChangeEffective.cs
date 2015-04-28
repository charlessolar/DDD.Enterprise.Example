using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/item/{ItemId}/effective", "POST")]
    public class ChangeEffective : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid ItemId { get; set; }

        public DateTime Effective { get; set; }
    }
}