using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/draft", "POST")]
    public class ChangeSkipDraft : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Boolean SkipDraft { get; set; }
    }
}