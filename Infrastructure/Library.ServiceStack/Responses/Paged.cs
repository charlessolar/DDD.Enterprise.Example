using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Library.Responses
{
    public class Paged<T>
    {
        public IEnumerable<T> Records { get; set; }

        public Int64 Total { get; set; }

        public Int32 ElapsedMs { get; set; }
    }

    // A pages response has just 1 subscription
    // On the client, they will receive an update when any of the paged results is updated
    // Its up to the client's viewmodel to update the correct array element
}