using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Services
{
    [Api("Relations")]
    [Route("/relations/store/{StoreId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid StoreId { get; set; }
    }
}