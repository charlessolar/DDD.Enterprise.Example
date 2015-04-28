using System;

namespace Demo.Domain.Relations.Store
{
    public interface IStore : Aggregates.Contracts.IEventSource<Guid>
    {
    }
}