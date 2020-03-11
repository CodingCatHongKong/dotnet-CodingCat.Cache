using CodingCat.Cache.Redis.Interfaces;
using StackExchange.Redis;
using System;

namespace CodingCat.Cache.Redis
{
    public class RedisStorageConfiguration : IRedisStorageConfiguration
    {
        public IConnectionMultiplexer RedisConnection { get; set; }

        public int DatabaseIndex { get; set; } = -1;
        public TimeSpan Expiry { get; set; }

        #region Constructor(s)

        public RedisStorageConfiguration()
        {
        }

        public RedisStorageConfiguration(
            IConnectionMultiplexer redisConnection,
            RedisStorageConfiguration configuration
        )
        {
            this.RedisConnection = redisConnection;
            this.DatabaseIndex = configuration.DatabaseIndex;
            this.Expiry = configuration.Expiry;
        }

        #endregion Constructor(s)

        public IDatabase GetDatabase()
        {
            return this.RedisConnection
                .GetDatabase(this.DatabaseIndex);
        }
    }
}