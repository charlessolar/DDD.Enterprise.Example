using Demo.Library.Queries;
using System;

namespace Demo.Application.RavenDB.Authentication.Users.Queries
{
    public class Get : BasicQuery
    {
        public String Id { get; set; }
    }
}