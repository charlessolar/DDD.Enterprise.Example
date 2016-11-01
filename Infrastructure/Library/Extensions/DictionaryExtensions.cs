using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Extensions
{
    public static class DictionaryExtensions
    {
        public static int GetContentsHash<TKey, TValue>(this IDictionary<TKey, TValue> obj)
        {
            int hash = 0;

            foreach (var pair in obj)
            {
                int key = pair.Key.GetHashCode(); // key cannot be null
                int value = pair.Value != null ? pair.Value.GetHashCode() : 0;
                hash ^= ShiftAndWrap(key, 2) ^ value;
            }

            return hash;
        }

        private static int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer. 
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded. 
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits. 
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
        public static T MergeLeft<T, TK, TV>(this T me, params IDictionary<TK, TV>[] others)
            where T : IDictionary<TK, TV>, new()
        {
            T newMap = new T();
            foreach (IDictionary<TK, TV> src in
                (new List<IDictionary<TK, TV>> { me }).Concat(others))
            {
                // ^-- echk. Not quite there type-system.
                foreach (KeyValuePair<TK, TV> p in src)
                {
                    newMap[p.Key] = p.Value;
                }
            }
            return newMap;
        }
        public static IDictionary<T, TU> Merge<T, TU>(this IDictionary<T, TU> first, IDictionary<T, TU> second, bool deep) where TU : struct
        {
            var results = new Dictionary<T, TU>();

            foreach (var item in first)
                results[item.Key] = item.Value;
            foreach (var item in second)
                results[item.Key] = item.Value;
            return results;
        }
    }
}
