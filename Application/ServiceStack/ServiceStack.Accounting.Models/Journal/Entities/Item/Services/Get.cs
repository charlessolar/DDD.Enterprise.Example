using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Services
{
    [Api("Accounting")]
    [Route("/accounting/journal/{JournalId}/item/{ItemId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid JournalId { get; set; }

        public Guid ItemId { get; set; }
    }
}