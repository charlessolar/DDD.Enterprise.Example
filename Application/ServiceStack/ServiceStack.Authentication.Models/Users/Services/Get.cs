using Demo.Library.Queries;
using ServiceStack;
using ServiceStack.Model;
using System;

namespace Demo.Application.ServiceStack.Authentication.Users.Services
{
    [Api("getUser")]
    [Route("/user", "GET")]
    public class Get : Query<Responses.Get>
    {
    }
}