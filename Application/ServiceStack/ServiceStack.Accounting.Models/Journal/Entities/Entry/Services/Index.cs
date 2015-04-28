using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/entries", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
        public Guid JournalId { get; set; }
    }
}