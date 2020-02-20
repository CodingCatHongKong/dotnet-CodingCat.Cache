using CodingCat.Cache.Redis.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingCat.Cache.Tests.Impls
{
    class ConnectRedisConfiguration : IConnectRedisConfiguration
    {
        public TimeSpan TimeoutPerTry { get; set; }
        public TimeSpan RetryInterval { get; set; }
        public uint RetryUpTo { get; set; }
    }
}
