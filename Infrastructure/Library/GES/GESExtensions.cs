using EventStore.ClientAPI;
using Demo.Library.GES;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class GESExtensions
    {
        public static EventData ToEventData(this Object e)
        {
            var data = Json.ToJsonBytes(e);

            var typeName = e.GetType().Name;
            return new EventData(Guid.NewGuid(), typeName, true, data, null);
        }
    }
}