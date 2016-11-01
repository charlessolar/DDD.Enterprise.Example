using EventStore.ClientAPI;
using Demo.Library.GES;
using System;

namespace Demo.Library.Extensions
{
    public static class GesExtensions
    {
        public static EventData ToEventData(this object e)
        {
            var data = e.ToJsonBytes();

            var typeName = e.GetType().Name;
            return new EventData(Guid.NewGuid(), typeName, true, data, null);
        }
    }
}