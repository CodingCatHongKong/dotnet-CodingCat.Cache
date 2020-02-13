using CodingCat.Cache.Enums;
using System;

namespace CodingCat.Cache.Interfaces
{
    public interface IStorageManager : IStorage
    {
        IStorage DefaultStorage { get; }
        IStorage[] Fallbacks { get; }

        FallbackPolicy FallbackPolicy { get; }

        IStorageManager SetDefault(IStorage storage);
        IStorageManager AddFallback(IStorage storage);

        string Get(IKeyBuilder key, FallbackPolicy fallbackPolicy);
        string Get(IKeyBuilder key, Func<string> callback, FallbackPolicy fallbackPolicy);
    }
}
