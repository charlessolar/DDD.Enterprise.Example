using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/subscriptions", "GET")]
    public class Subscriptions : QueriesPaged<Responses.Subscriptions>
    {
    }
}
