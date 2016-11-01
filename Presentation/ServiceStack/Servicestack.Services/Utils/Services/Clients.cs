using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/clients", "GET")]
    public class Clients : QueriesPaged<Responses.Client>
    {
        public string Username { get; set; }
    }
}
