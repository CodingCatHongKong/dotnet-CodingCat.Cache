using CodingCat.Cache.Redis.Interfaces;
using System;

namespace CodingCat.Cache.Tests.Impls
{
    internal class ConnectRedisConfiguration : IConnectRedisConfiguration
    {
        public TimeSpan TimeoutPerTry { get; set; }
        public TimeSpan RetryInterval { get; set; }
        public uint RetryUpTo { get; set; }
    }
}