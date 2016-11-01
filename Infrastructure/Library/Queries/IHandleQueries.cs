using NServiceBus;

namespace Demo.Library.Queries
{
    public interface IHandleQueries<TQuery> : IHandleMessages<TQuery> where TQuery : IQuery
    {
    }
}
