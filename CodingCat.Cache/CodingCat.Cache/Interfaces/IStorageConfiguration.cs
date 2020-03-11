using System;

namespace CodingCat.Cache.Interfaces
{
    public interface IStorageConfiguration
    {
        TimeSpan Expiry { get; }
    }
}