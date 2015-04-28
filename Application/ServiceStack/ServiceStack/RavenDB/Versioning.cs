using Raven.Client.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Application.Servicestack.RavenDB
{
    public class Versioning : IDocumentStoreListener
    {
        public void AfterStore(string key, object entityInstance, Raven.Json.Linq.RavenJObject metadata)
        {
        }

        public bool BeforeStore(string key, object entityInstance, Raven.Json.Linq.RavenJObject metadata, Raven.Json.Linq.RavenJObject original)
        {
            if (!metadata.ContainsKey("Version"))
                metadata["Version"] = 0;
            metadata["Version"] = metadata["Version"].Value<Int32>() + 1;

            return true;
        }
    }
}