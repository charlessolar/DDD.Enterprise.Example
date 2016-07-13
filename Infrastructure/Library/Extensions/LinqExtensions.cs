using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public class KeyEqualityComparer<T> : IEqualityComparer<T>
    {
        protected readonly Func<T, T, bool> comparer;
        protected readonly Func<T, object> keyExtractor;

        // Enable to only specify the key to compare with: y => y.CustomerID
        public KeyEqualityComparer(Func<T, object> keyExtractor)
            : this(keyExtractor, null)
        {
        }
        // Enable to specify how to tel if two objects are equal: (x, y) => y.CustomerID == x.CustomerID
        public KeyEqualityComparer(Func<T, T, bool> comparer)
            : this(null, comparer)
        {
        }

        public KeyEqualityComparer(Func<T, object> keyExtractor, Func<T, T, bool> comparer)
        {
            this.keyExtractor = keyExtractor;
            this.comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            if (comparer != null)
                return comparer(x, y);
            else
            {
                var valX = keyExtractor(x);
                if (valX is IEnumerable<object>) // The special case where we pass a list of keys
                    return ((IEnumerable<object>)valX).SequenceEqual((IEnumerable<object>)keyExtractor(y));

                return valX.Equals(keyExtractor(y));
            }
        }

        public int GetHashCode(T obj)
        {
            if (keyExtractor == null)
                return obj.ToString().ToLower().GetHashCode();
            else
            {
                var val = keyExtractor(obj);
                if (val is IEnumerable<object>) // The special case where we pass a list of keys
                    return (int)((IEnumerable<object>)val).Aggregate((x, y) => x.GetHashCode() ^ y.GetHashCode());

                return val.GetHashCode();
            }
        }
    }

    public class Comparer<T> : IComparer<T>
    {
        protected readonly Func<T, T, int> _comparer;

        public Comparer(Func<T, T, int> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            _comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return _comparer(x, y);
        }
    }
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

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> list, Func<T, object> keyExtractor)
        {
            return list.Distinct(new KeyEqualityComparer<T>(keyExtractor));
        }

        public static bool Contains<T>(this IEnumerable<T> list, T item, Func<T, object> keyExtractor)
        {
            return list.Contains(item, new KeyEqualityComparer<T>(keyExtractor));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, IEnumerable<T> except, Func<T, object> keyExtractor)
        {
            return list.Except(except, new KeyEqualityComparer<T>(keyExtractor));
        }

        public static IEnumerable<T> Intersect<T>(this IEnumerable<T> list, IEnumerable<T> toIntersect, Func<T, object> keyExtractor)
        {
            return list.Intersect(toIntersect, new KeyEqualityComparer<T>(keyExtractor));
        }

        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector, Func<TKey, TKey, int> comparer)
        {
            return list.OrderBy(keySelector, new Comparer<TKey>(comparer));
        }

        public static IOrderedEnumerable<T> OrderByDescending<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector, Func<TKey, TKey, int> comparer)
        {
            return list.OrderByDescending(keySelector, new Comparer<TKey>(comparer));
        }

        public static bool SequenceEqual<T>(this IEnumerable<T> list, IEnumerable<T> sequenceToEqual, Func<T, object> keyExtractor)
        {
            return list.SequenceEqual(sequenceToEqual, new KeyEqualityComparer<T>(keyExtractor));
        }

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, object> keyExtractor)
        {
            return first.Union(second, new KeyEqualityComparer<TSource>(keyExtractor));
        }
        public static IEnumerable<TSource> UnionReplace<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, object> keyExtractor) where TSource : class
        {
            var sorted = second.OrderBy(x => keyExtractor(x));
            return first.Select(x =>
            {
                var exist = sorted.FirstOrDefault(y => keyExtractor(y) == keyExtractor(x));
                return exist ?? x;
            });
        }

        /// <summary>
        /// Iterate over a list, skipping [step] elements along the way
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static IEnumerable<T> Jump<T>(this IEnumerable<T> data, int step)
        {
            int pos = 0;
            while (pos <= data.Count())
            {
                yield return data.ElementAt(pos);
                pos += step;
            }
        }

        /// <summary>
        /// Iterate over a list with a datatime element, skipping over items inside a certain timespan
        /// Ie:  Give me an element for each hour, etc.
        /// Works ASCENDING
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="selector">selector to retreive the datetime from T</param>
        /// <param name="step">timespan to step</param>
        /// <returns></returns>
        public static IEnumerable<T> Jump<T>(this IEnumerable<T> data, Func<T, DateTime> selector, TimeSpan step)
        {
            var ordered = data.ToList().OrderBy(selector);

            int pos = 0;
            while (pos < data.Count())
            {
                yield return ordered.ElementAt(pos);
                var target = selector(ordered.ElementAt(pos)).Add(step);

                var indexStep = ordered.Skip(pos).Count(x => selector(x) < target);
                pos += Math.Max(1, indexStep);
            }
        }

        /// <summary>
        /// Iterate over a list with a datatime element, skipping over items inside a certain timespan
        /// Ie:  Give me an element for each hour, etc.
        /// Works DESCENDING
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="selector">selector to retreive the datetime from T</param>
        /// <param name="step">timespan to step</param>
        /// <returns></returns>
        public static IEnumerable<T> JumpDescending<T>(this IEnumerable<T> data, Func<T, DateTime> selector, TimeSpan step)
        {
            var ordered = data.ToList().OrderByDescending(selector);

            int pos = 0;
            while (pos < data.Count())
            {
                yield return ordered.ElementAt(pos);
                var target = selector(ordered.ElementAt(pos)).Subtract(step);

                var indexStep = ordered.Skip(pos).Count(x => selector(x) > target);
                pos += Math.Max(1, indexStep);
            }
        }

        public static double? Median<TColl, TValue>(
    this IEnumerable<TColl> source,
    Func<TColl, TValue> selector)
        {
            return source.Select<TColl, TValue>(selector).Median();
        }

        public static double? Median<T>(
            this IEnumerable<T> source)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
                source = source.Where(x => x != null);

            int count = source.Count();
            if (count == 0)
                return null;

            source = source.OrderBy(n => n);

            int midpoint = count / 2;
            if (count % 2 == 0)
                return (Convert.ToDouble(source.ElementAt(midpoint - 1)) + Convert.ToDouble(source.ElementAt(midpoint))) / 2.0;
            else
                return Convert.ToDouble(source.ElementAt(midpoint));
        }
        public static TValue Mode<TColl, TValue>(
    this IEnumerable<TColl> source,
    Func<TColl, TValue> selector)
        {
            return source.Select<TColl, TValue>(selector).Mode();
        }
        public static T Mode<T>(this IEnumerable<T> list)
        {
            // Initialize the return value
            T mode = default(T);

            // Test for a null reference and an empty list
            if (list != null && list.Count() > 0)
            {
                // Store the number of occurences for each element
                Dictionary<T, int> counts = new Dictionary<T, int>();

                // Add one to the count for the occurence of a character
                foreach (T element in list)
                {
                    if (counts.ContainsKey(element))
                        counts[element]++;
                    else
                        counts.Add(element, 1);
                }

                // Loop through the counts of each element and find the 
                // element that occurred most often
                int max = 0;

                foreach (KeyValuePair<T, int> count in counts)
                {
                    if (count.Value > max)
                    {
                        // Update the mode
                        mode = count.Key;
                        max = count.Value;
                    }
                }
            }

            return mode;
        }

        public static double Percentile<TColl, TValue>(
    this IEnumerable<TColl> source,
    Func<TColl, TValue> selector, double percentile)
        {
            return source.Select<TColl, TValue>(selector).Percentile(percentile);
        }
        public static double Percentile<T>(this IEnumerable<T> sequence, double percentile)
        {
            var sorted = sequence.OrderBy(x => x);

            int N = sorted.Count();
            double n = (N - 1) * percentile + 1;

            // Another method: double n = (N + 1) * percentile;
            if (n == 1d) return Convert.ToDouble(sorted.ElementAt(0));
            else if (n == N) return Convert.ToDouble(sorted.ElementAt(N - 1));
            else
            {
                int k = (int)n;
                double d = n - k;
                return Convert.ToDouble(sorted.ElementAt(k - 1)) + d * (Convert.ToDouble(sorted.ElementAt(k)) - Convert.ToDouble(sorted.ElementAt(k - 1)));
            }
        }

    }
}