using Demo.Library.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Demo.Domain.Infrastructure.Extensions
{
    public static class PagedExtensions
    {
        public static string ToId(this IPaged paged)
        {
            if (paged == null) return "{}";
            var properties = paged.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x =>
                {
                    var val = x.GetValue(paged);
                    if (val == null) return x.Name + ":";
                    return x.Name + ":" + val.ToString();
                });
            
            var id = properties.Aggregate((cur, next) => $"{cur};{next}");
            return $"{{{id}}}";
        }
        public static string ToId(this object paged)
        {
            if (paged == null) return "{}";

            var properties = paged.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x =>
                {
                    var val = x.GetValue(paged);
                    if (val == null) return x.Name + ":";
                    return x.Name + ":" + val.ToString();
                });
            var id = properties.Aggregate((cur, next) => $"{cur};{next}");
            return $"{{{id}}}";
        }
        public static string ToId(this IDictionary<string, string> paged)
        {
            if (paged == null) return "{}";
            
            var id = paged.Select(x => x.Key + ":" + x.Value).Aggregate((cur, next) => $"{cur};{next}");
            return $"{{{id}}}";
        }
        public static string ToId(this IDictionary<string, object> paged)
        {
            if (paged == null) return "{}";

            var id = paged.Select(x => x.Key + ":" + x.Value.ToString()).Aggregate((cur, next) => $"{cur};{next}");
            return $"{{{id}}}";
        }
    }
}
