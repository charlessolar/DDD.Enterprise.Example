using Demo.Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Riak.Infrastructure.Extensions
{
    public static class PagedExtensions
    {
        public static String ToId(this IPaged paged)
        {
            var properties = paged.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(x =>
                {
                    var val = x.GetValue(paged);
                    if (val == null) return null;
                    return x.Name + ":" + val.ToString();
                }).Where(x => x != null);

            if (properties.Count() == 0) return "";

            return properties.Aggregate((cur, next) => $"{cur};{next}");
        }
    }
}
