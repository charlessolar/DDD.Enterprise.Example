using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/memory", "GET")]
    public class Memory : QueriesQuery<Responses.Memory>
    {
    }
}
