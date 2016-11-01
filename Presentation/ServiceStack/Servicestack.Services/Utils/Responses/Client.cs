using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Utils.Responses
{
    public class Client : IHasStringId
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string[] Channels { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string SessionId { get; set; }
        public string UserAddress { get; set; }
        public bool IsAuthenticated { get; set; }

        public Dictionary<string, string> Meta { get; set; }
    }
}
