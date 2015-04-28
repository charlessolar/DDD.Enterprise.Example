using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/entry/{EntryId}/reviewed", "POST")]
    public class Reviewed : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid EntryId { get; set; }

        public Guid EmployeeId { get; set; }
    }
}