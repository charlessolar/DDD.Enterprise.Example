using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> Distinct<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<TSource> Except<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer)
        {
            var keys = new HashSet<TKey>(second.Select(keySelector), keyComparer);
            foreach (var element in first)
            {
                var key = keySelector(element);
                if (keys.Contains(key))
                {
                    continue;
                }
                yield return element;
                keys.Add(key);
            }
        }
    }
}