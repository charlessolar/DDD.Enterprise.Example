using Demo.Presentation.ServiceStack.Infrastructure.Cache;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class CacheExtensions
    {
        /// <summary>
        /// Wraps an object into a cachable object
        /// </summary>
        /// <param name="obj">The object to be wrapped</param>
        /// <param name="version">Optional initial version</param>
        /// <returns>A wrapper object to serialize into cache</returns>
        public static Wrapper<T> Wrap<T>(this T obj, int version = 0) where T : class, IHasGuidId
        {
            return new Wrapper<T>
            {
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Version = version,
                Payload = obj
            };
        }

        public static Wrapper<T> AddSession<T>(this T obj, ICacheClient cache, string session) where T : class, IHasGuidId
        {
            var key = UrnId.Create<T>(obj.Id);

            var cached = key.FromCache<T>(cache)
                ?? obj.Wrap();

            if (cached.Sessions == null)
                cached.Sessions = new List<string>();

            // Don't allow duplicate session strings, so remove if it exists first
            cached.Sessions.Remove(session);
            cached.Sessions.Add(session);

            cached.UpdateCache(cache, key);

            return cached;
        }

        public static void RemoveSession<T>(this T obj, ICacheClient cache, string session) where T : class, IHasGuidId
        {
            var key = UrnId.Create<T>(obj.Id);

            var cached = key.FromCache<T>(cache);

            if (cached?.Sessions == null) return;

            cached.Sessions.Remove(session);

            cached.UpdateCache(cache, key);
        }

        public static Wrapper<T> FromCache<T>(this string urn, ICacheClient cache) where T : class, IHasGuidId
        {
            var cached = cache.Get<string>(urn);
            return cached?.FromJson<Wrapper<T>>();
        }

        public static void AddCache<T>(this Wrapper<T> wrapper, ICacheClient cache, string key) where T : class, IHasGuidId
        {
            cache.Add(key, wrapper.ToJson());
        }

        public static void UpdateCache<T>(this Wrapper<T> wrapper, ICacheClient cache, string key) where T : class, IHasGuidId
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

            Wrapper<T> wrapper = key.FromCache<T>(cache)
                ?? obj.Wrap();

            wrapper.Payload = obj;
            wrapper.Version++;

            cache.Set(key, wrapper.ToJson());
        }
    }
}