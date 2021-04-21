using System;
using System.Runtime.Caching;

namespace Cache
{
    public class Cache<T>
    {
        private readonly ObjectCache cache = MemoryCache.Default;
        private double dt_seconds = 0;
        private readonly CacheItemPolicy cacheItemPolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5),
        };

        public Cache(double dt_seconds)
        {
            this.dt_seconds = dt_seconds;
        }

        public T Get(string Key)
        {
            if (cache.Get(Key) == null) return default(T);
            return (T)cache.Get(Key);
        }

        public void Set(string Key, T Data)
        {
            if (this.dt_seconds > 0) cache.Add(Key, Data, DateTimeOffset.Now.AddSeconds(dt_seconds));
            else cache.Set(Key, Data, cacheItemPolicy);
        }
    }
}
