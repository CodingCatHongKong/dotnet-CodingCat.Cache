using CodingCat.Cache.Interfaces;
using System;
using System.Runtime.Caching;

namespace CodingCat.Cache.Memory
{
    public class Storage : IStorage
    {
        private MemoryCache memoryCache { get; }

        public TimeSpan Expiry { get; }

        #region Constructor(s)

        public Storage(TimeSpan expiry)
        {
            this.memoryCache = MemoryCache.Default;

            this.Expiry = expiry;
        }

        #endregion Constructor(s)

        public IStorage Add(IKeyBuilder key, string item)
        {
            this.memoryCache.Add(
                key.ToString(),
                item,
                DateTimeOffset.Now.Add(this.Expiry)
            );
            return this;
        }

        public string Get(IKeyBuilder key)
        {
            if (this.Exists(key))
            {
                return this.memoryCache[key.ToString()]
                    ?.ToString();
            }

            return null;
        }

        public string Get(IKeyBuilder key, Func<string> callback)
        {
            return this.Get(key) ?? this.Add(key, callback()).Get(key);
        }

        public IStorage Delete(IKeyBuilder key)
        {
            if (this.Exists(key)) this.memoryCache.Remove(key.ToString());
            return this;
        }

        private bool Exists(IKeyBuilder key)
        {
            return this.memoryCache.Contains(key.ToString());
        }
    }
}