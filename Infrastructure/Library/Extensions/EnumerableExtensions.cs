using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> TryAdd(this IEnumerable<string> source, string element)
        {
            if (source == null)
                source = new string[] { };

            if (source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret;
        }
        public static IEnumerable<string> TryAdd(this IEnumerable<string> source, ValueType element)
        {
            if (source == null)
                source = new string[] { };

            if (source.Contains(element.ToString()))
                return source;

            var ret = source.ToList();
            ret.Add(element.ToString());
            return ret;
        }
        public static IEnumerable<T> TryAdd<T, U>(this IEnumerable<T> source, T element, Func<T, U> selector)
        {
            if (source == null)
                source = new T[] { };

            if (source.Any(x => selector.Invoke(x).Equals(selector.Invoke(element))))
                return source;

            var ret = source.ToList();
            ret.Add(element);
            return ret;
        }
        public static IEnumerable<T> Add<T>(this IEnumerable<T> source, T element)
        {
            if (source == null)
                source = new T[] { };

            var ret = source.ToList();
            ret.Add(element);
            return ret;
        }
        public static IEnumerable<string> TryRemove(this IEnumerable<string> source, string element)
        {
            if (source == null)
                source = new string[] { };

            if (!source.Contains(element))
                return source;

            var ret = source.ToList();
            ret.Remove(element);
            return ret;
        }
        public static IEnumerable<string> TryRemove(this IEnumerable<string> source, ValueType element)
        {
            if (source == null)
                source = new string[] { };

            if (!source.Contains(element.ToString()))
                return source;

            var ret = source.ToList();
            ret.Remove(element.ToString());
            return ret;
        }
        public static IEnumerable<T> TryRemove<T,U>(this IEnumerable<T> source, T element, Func<T, U> selector)
        {
            if (source == null)
                source = new T[] { };

            var idx = source.SingleOrDefault(x => selector.Invoke(x).Equals(selector.Invoke(element)));
            if (idx == null)
                return source;

            var ret = source.ToList();
            ret.Remove(idx);
            return ret;
        }
        public static IEnumerable<T> TryRemove<T, K>(this IEnumerable<T> source, K key, Func<T, K> selector)
        {
            if (source == null)
                source = new T[] { };

            var idx = source.SingleOrDefault(x => selector.Invoke(x).Equals(key));
            if (idx == null)
                return source;

            var ret = source.ToList();
            ret.Remove(idx);
            return ret;
        }
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            for (var i = 0; i < source.Count(); i++)
                action(source.ElementAt(i));
        }
        public static int GetOrderIndependentHashCode<T>(this IEnumerable<T> source)
        {
            int hash = 0;
            foreach (T element in source)
            {
                hash = hash ^ EqualityComparer<T>.Default.GetHashCode(element);
            }
            return hash;
        }
    }
}

