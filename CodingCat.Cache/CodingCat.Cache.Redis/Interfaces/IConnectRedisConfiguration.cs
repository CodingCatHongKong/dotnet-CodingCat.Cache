using System;

namespace CodingCat.Cache.Redis.Interfaces
{
    public interface IConnectRedisConfiguration
    {
        TimeSpan TimeoutPerTry { get; }
        TimeSpan RetryInterval { get; }
        uint RetryUpTo { get; }
    }
}