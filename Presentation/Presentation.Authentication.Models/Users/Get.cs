using Demo.Library.Queries;
using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.Model;
using System;

namespace Demo.Presentation.Authentication.Models.Users
{
    [Api("Authentication")]
    [Route("/user", "GET")]
    public class Get : BasicQuery, IReturn<GetResponse>
    {
    }

    public class GetResponse : Base, IHasStringId
    {
        public String Id { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String NickName { get; set; }

        public String ImageData { get; set; }
    }
}