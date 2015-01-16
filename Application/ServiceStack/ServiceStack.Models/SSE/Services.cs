using ServiceStack;
using System;

namespace Forte.Application.ServiceStack.Models.Cache.Services
{
    [Route("/subscribe", "POST")]
    public class Subscribe
    {
        public Guid QueryId { get; set; }
        public String Receiver { get; set; }
        public Int32? Timeout { get; set; }
    }

    [Route("/unsubscribe", "POST")]
    public class Unsubscribe
    {
        public Guid QueryId { get; set; }
    }
}