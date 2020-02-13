using System;

namespace CodingCat.Cache.Interfaces
{
    public interface IStorage
    {
        IStorage Add(IKeyBuilder key, string item);

        string Get(IKeyBuilder key);

        string Get(IKeyBuilder usingKey, Func<string> callback);

        IStorage Delete(IKeyBuilder key);
    }
}