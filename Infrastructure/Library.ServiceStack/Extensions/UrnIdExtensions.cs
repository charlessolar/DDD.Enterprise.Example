using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class UrnIdExtensions
    {
        public static String GetCacheKey<TResponse>(this Query<TResponse> query)
        {
            var properties = query.GetType()
                .GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Select(x =>
                {
                    var val = x.GetValue(query);
                    if (val == null) return x.Name + "/";
                    return x.Name + "/" + val.ToString();
                });

            return "urn:{0}:{1}".Fmt(typeof(TResponse).FullName, properties.Join(":"));
        }
        public static String GetCacheKey<TResponse>(this PagedQuery<TResponse> query)
        {
            var properties = query.GetType()
                .GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Select(x =>
                {
                    var val = x.GetValue(query);
                    if (val == null) return x.Name + "/";
                    return x.Name + "/" + val.ToString();
                });

            return "urn:{0}:{1}".Fmt(typeof(TResponse).FullName, properties.Join(":"));
        }
    }
}