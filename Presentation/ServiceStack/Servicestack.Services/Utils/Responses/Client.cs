using Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Utils.Responses
{
    public class Client : IHasStringId
    {
        public String Id { get; set; }

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
