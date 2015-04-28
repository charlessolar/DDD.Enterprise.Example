using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/name", "POST")]
    public class ChangeName : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }

        public String Name { get; set; }
    }
}