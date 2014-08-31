using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Cache
{
    public static class CacheExtensions
    {
        /// <summary>
        /// Wraps an object into a cachable object
        /// </summary>
        /// <param name="obj">The object to be wrapped</param>
        /// <param name="version">Optional initial version</param>
        /// <returns>A wrapper object to serialize into cache</returns>
        public static Wrapper Wrap(this IHasGuidId obj, Int32 version = 0)
        {
            return new Wrapper
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Version = version,
                Payload = obj
            };
        }




        public static void AddSession<T>( this T obj, ICacheClient cache, String session) where T : class, IHasGuidId
        {
            var key = UrnId.Create<T>(obj.Id);

            var cached = key.FromCache(cache) 
                ?? obj.Wrap();

            if (cached == null) return;
            if (cached.Sessions == null)
                cached.Sessions = new List<String>();

            cached.Sessions.Add(session);

            cached.UpdateCache(cache, key);
        }

        public static void RemoveSession<T>( this T obj, ICacheClient cache, String session) where T : class, IHasGuidId
        {
            var key = UrnId.Create<T>(obj.Id);

            var cached = key.FromCache(cache);

            if (cached == null || cached.Sessions == null) return;

            cached.Sessions.Remove(session);

            cached.UpdateCache(cache, key);
        }

        public static Wrapper FromCache( this String urn, ICacheClient cache)
        {
            var cached = cache.Get<String>(urn);
            if (cached == null) return null;
            return cached.FromJson<Wrapper>();
        }


        public static void AddCache(this Wrapper wrapper, ICacheClient cache, String key)
        {
            cache.Add(key, wrapper.ToJson());
        }
        public static void UpdateCache( this Wrapper wrapper, ICacheClient cache, String key )
        {
            cache.Set(key, wrapper.ToJson());
        }
        public static void AddCache<T>(this T obj, ICacheClient cache) where T : class, IHasGuidId
        {
            var key = UrnId.Create<T>(obj.Id);

            var wrapper = obj.Wrap();

            cache.Add(key, wrapper.ToJson());
        }
        public static void UpdateCache<T>(this T obj, ICacheClient cache) where T : class, IHasGuidId
        {
            var key = UrnId.Create<T>(obj.Id);

            Wrapper wrapper = key.FromCache(cache)
                ?? obj.Wrap();

            wrapper.Payload = obj;
            wrapper.Version++;

            cache.Set(key, wrapper.ToJson());
        }
    }
}
