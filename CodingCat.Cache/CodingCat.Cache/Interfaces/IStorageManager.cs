using CodingCat.Cache.Enums;
using System;

namespace CodingCat.Cache.Interfaces
{
    public interface IStorageManager : IStorage
    {
        FallbackPolicy FallbackPolicy { get; }

        string Get(IKeyBuilder key, FallbackPolicy fallbackPolicy);
        string Get(IKeyBuilder key, Func<string> callback, FallbackPolicy fallbackPolicy);
    }
}
