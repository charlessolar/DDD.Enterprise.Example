using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/check", "POST")]
    public class ChangeCheckDate : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Boolean CheckDate { get; set; }
    }
}