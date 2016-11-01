using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Library.Demo
{
    public interface IDemo
    {
        Task Init(string url, Guid stashId, string secret, string name, string description);
        Task Report(string Event, object data);
        Task Report(string Event, IDictionary<string, object> data);
    }
}
