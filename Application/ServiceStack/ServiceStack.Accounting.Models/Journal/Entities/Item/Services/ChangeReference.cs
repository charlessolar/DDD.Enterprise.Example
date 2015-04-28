using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/item/{ItemId}/reference", "POST")]
    public class ChangeReference : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid ItemId { get; set; }

        public String Reference { get; set; }
    }
}