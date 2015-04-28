using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Journal.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid JournalId { get; set; }
    }
}