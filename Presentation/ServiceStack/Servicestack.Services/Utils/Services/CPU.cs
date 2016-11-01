using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/cpu", "GET")]
    public class Cpu : QueriesQuery<Responses.Cpu>
    {
    }
}
