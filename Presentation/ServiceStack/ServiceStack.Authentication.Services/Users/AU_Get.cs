using System;
using ServiceStack;
using ServiceStack.Model;
using Demo.Presentation.ServiceStack.Infrastructure.Queries;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Models
{
    [Api("User")]
    [Route("/user/{UserAuthId}", "GET")]
    public class AU_Get : Queries_Query<Models.AU_UserResponse>, Queries.Get
    {
        public string UserAuthId { get; set; }
    }
}