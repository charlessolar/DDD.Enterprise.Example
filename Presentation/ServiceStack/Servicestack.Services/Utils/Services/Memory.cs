using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/memory", "GET")]
    public class Memory : Queries_Query<Responses.Memory>
    {
    }
}
