using ServiceStack;
using Demo.Presentation.ServiceStack.Infrastructure.Queries;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("User")]
    [Route("/user/{UserAuthId}", "GET")]
    public class AuGet : QueriesQuery<Models.AuUserResponse>, Queries.IGet
    {
        public string UserAuthId { get; set; }
    }
}