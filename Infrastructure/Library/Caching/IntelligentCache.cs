using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aggregates.Contracts;
using NLog;

namespace Demo.Library.Caching
{
    public class IntelligentCache : IDisposable
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static readonly ConcurrentDictionary<string, object> MemCache =
            new ConcurrentDictionary<string, object>();


        // For streams that are changing multiple times a second, no sense to cache them if they get immediately evicted
        private static readonly HashSet<string> Uncachable = new HashSet<string>();
        private static readonly HashSet<string> LevelOne = new HashSet<string>();
        private static readonly HashSet<string> LevelZero = new HashSet<string>();
        private static int _stage;
        private static readonly Timer _uncachableEviction = new Timer(_ =>
        {
            // Clear uncachable every 5 minutes
            if (_stage == 60)
            {
                _stage = 0;
                Logger.Debug($"Clearing {Uncachable.Count} uncachable keys");
                Uncachable.Clear();
            }
            // Clear levelOne every 15 seconds
            if (_stage % 3 == 0)
                LevelOne.Clear();

            // Clear levelZero every 5 seconds
            LevelZero.Clear();

            _stage++;
        }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

        private readonly bool _intelligent;
        private bool _disposed;

        public IntelligentCache(bool intelligent=true)
        {
            _intelligent = intelligent;
        }

        public void Cache(string key, object cached)
        {
            if (_intelligent && (Uncachable.Contains(key) || LevelOne.Contains(key)))
                return;

            Logger.Debug($"Caching [{key}]");
            MemCache.AddOrUpdate(key, (_) => cached, (_, e) => cached);
        }

        public object GetOrCache(string key, Func<object> factory)
        {
            return MemCache.GetOrAdd(key, (_) => factory());
        }
        public void Evict(string key)
        {
            Logger.Debug($"Evicting key [{key}] from cache");
            if (_intelligent)
            {
                if (Uncachable.Contains(key)) return;

                if (LevelZero.Contains(key))
                {
                    if (LevelOne.Contains(key))
                    {
                        Logger.Debug($"Key [{key}] has been evicted frequenty, marking uncachable for a few minutes");
                        Uncachable.Add(key);
                    }
                    else
                        LevelOne.Add(key);
                }
                else
                    LevelZero.Add(key);

            }
            object e;
            MemCache.TryRemove(key, out e);
        }
        public object Retreive(string key)
        {
            object cached;
            if (!MemCache.TryGetValue(key, out cached))
                cached = null;
            if (cached == null)
                Logger.Debug($"Key [{key}] is not in cache");

            return cached;
        }

        public void Dispose()
        {
            if (!_disposed)
                _uncachableEviction.Dispose();
            _disposed = true;
        }
    }
}
