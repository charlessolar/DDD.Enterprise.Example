using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/responsible", "POST")]
    public class ChangeResponsible : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public Guid EmployeeId { get; set; }
    }
}