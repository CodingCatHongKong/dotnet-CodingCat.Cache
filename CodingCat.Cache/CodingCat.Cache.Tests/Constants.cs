using System;

namespace CodingCat.Cache.Tests
{
    public static class Constants
    {
        public const string USING_KEY_PREFIX = "UnitTest";
        public const int DEFAULT_EXPIRY_IN_SECONDS = 1;

        public const string REDIS_CONFIG = "127.0.0.1:6379";

        public static readonly TimeSpan Expiry = TimeSpan.FromSeconds(DEFAULT_EXPIRY_IN_SECONDS);
    }
}