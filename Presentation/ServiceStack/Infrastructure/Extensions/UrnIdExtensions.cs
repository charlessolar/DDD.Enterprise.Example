using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;
using System.Linq;
using System.Reflection;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class UrnIdExtensions
    {
        public static string GetCacheKey<TResponse>(this QueriesQuery<TResponse> query)
        {
            var properties = query.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x =>
                {
                    var val = x.GetValue(query);
                    if (val == null) return null;
                    return x.Name + ":" + val.ToString();
                }).Where(x => x != null);

            return "urn:{0}:{1}".Fmt(query.GetType().FullName, properties.Join(":"));
        }
        public static string GetCacheKey<TResponse>(this QueriesPaged<TResponse> query)
        {
            var properties = query.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x =>
                {
                    var val = x.GetValue(query);
                    if (val == null) return null;
                    return x.Name + ":" + val.ToString();
                }).Where(x => x != null);

            return "urn:{0}:{1}".Fmt(query.GetType().FullName, properties.Join(":"));
        }
    }
}