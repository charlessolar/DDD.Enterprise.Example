using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Infrastructure.Cache
{
    public class Wrapper<T> where T : IHasGuidId
    {
        public Wrapper()
        {
            Sessions = new List<string>();
        }

        public int Version { get; set; }

        public ICollection<string> Sessions { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public T Payload { get; set; }
    }
}