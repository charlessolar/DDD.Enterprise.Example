using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Guid ResponsibleId { get; set; }

        public Boolean CheckDate { get; set; }

        public Boolean SkipDraft { get; set; }
    }
}