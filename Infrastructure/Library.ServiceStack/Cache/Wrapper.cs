using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Library.Cache
{
    public class Wrapper<T> where T : IHasGuidId
    {
        public Wrapper()
        {
            Sessions = new List<String>();
        }

        public Int32 Version { get; set; }

        public ICollection<String> Sessions { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public T Payload { get; set; }
    }
}