using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/credit", "POST")]
    public class SetCreditAccount : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid? AccountId { get; set; }
    }
}