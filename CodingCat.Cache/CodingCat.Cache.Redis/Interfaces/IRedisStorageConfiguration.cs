using CodingCat.Cache.Interfaces;
using StackExchange.Redis;

namespace CodingCat.Cache.Redis.Interfaces
{
    public interface IRedisStorageConfiguration : IStorageConfiguration
    {
        IDatabase GetDatabase();
    }
}