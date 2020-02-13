using CodingCat.Cache.Enums;
using CodingCat.Cache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingCat.Cache.Impls
{
    public class StorageManager : IStorageManager
    {
        private List<IStorage> fallbacks { get; } = new List<IStorage>();
        private IStorage[] storages
        {
            get => new IStorage[] { this.DefaultStorage }
                .Concat(this.fallbacks)
                .ToArray();
        }

        public IStorage DefaultStorage { get; private set; }
        public IStorage[] Fallbacks => this.fallbacks.ToArray();

        public FallbackPolicy FallbackPolicy { get; }

        #region Constructor(s)
        public StorageManager()
        {
            this.FallbackPolicy = FallbackPolicy.Default;
        }

        public StorageManager(IStorage defaultStorage) : this()
        {
            this.DefaultStorage = defaultStorage;
        }

        public StorageManager(FallbackPolicy fallbackPolicy)
        {
            this.FallbackPolicy = fallbackPolicy;
        }

        public StorageManager(
            FallbackPolicy fallbackPolicy,
            IStorage defaultStorage
        ) : this(fallbackPolicy)
        {
            this.DefaultStorage = defaultStorage;
        }
        #endregion

        #region IStorage
        public IStorage Add(IKeyBuilder key, string item)
        {
            foreach (var storage in this.storages)
                storage.Add(key, item);
            return this;
        }

        public string Get(IKeyBuilder key) => this.Get(key, this.FallbackPolicy);

        public string Get(IKeyBuilder key, Func<string> callback)
        {
            return this.Get(key) ?? this.Add(key, callback()).Get(key);
        }

        public IStorage Delete(IKeyBuilder key)
        {
            foreach (var storage in this.storages) storage.Delete(key);
            return this;
        } 
        #endregion

        #region IStorageManager
        public IStorageManager SetDefault(IStorage storage)
        {
            this.DefaultStorage = storage;
            return this;
        }

        public IStorageManager AddFallback(IStorage storage)
        {
            this.fallbacks.Add(storage);
            return this;
        }

        public string Get(IKeyBuilder key, FallbackPolicy fallbackPolicy)
        {
            var value = this.DefaultStorage.Get(key);
            if (value != null) return value;

            var saveTo = new List<IStorage>() { this.DefaultStorage };
            foreach (var storage in this.fallbacks)
            {
                if ((value = storage.Get(key)) != null) break;
                saveTo.Add(storage);
            }

            if (fallbackPolicy != FallbackPolicy.SaveFromFallback)
                saveTo.Clear();

            if (value != null)
                foreach (var storage in saveTo)
                    storage.Add(key, value);

            return value;
        }
        #endregion
    }
}
