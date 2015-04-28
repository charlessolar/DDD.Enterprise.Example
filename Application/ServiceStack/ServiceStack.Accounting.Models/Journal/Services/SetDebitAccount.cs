using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/debit", "POST")]
    public class SetDebitAccount : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid? AccountId { get; set; }
    }
}