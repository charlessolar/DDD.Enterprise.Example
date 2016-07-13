using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class UrnIdExtensions
    {
        public static String GetCacheKey<TResponse>(this Queries_Query<TResponse> query)
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
        public static String GetCacheKey<TResponse>(this Queries_Paged<TResponse> query)
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