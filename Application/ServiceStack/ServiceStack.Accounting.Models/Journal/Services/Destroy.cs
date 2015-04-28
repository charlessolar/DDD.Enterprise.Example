using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}", "DELETE")]
    public class Destroy : IReturn<Base<Command>>
    {
        public Guid JournalId { get; set; }
    }
}