using CodingCat.Cache.Redis;
using CodingCat.Cache.Tests.Abstracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System;

namespace CodingCat.Cache.Tests
{
    [TestClass]
    public class UnitTestRedisCache : BaseCachingTest<UnitTestRedisCache>
    {
        private IConnectionMultiplexer redis { get; }

        #region Constructor(s)

        public UnitTestRedisCache() : base()
        {
            this.redis = ConnectionMultiplexer
                .Connect(Constants.REDIS_CONFIG);
        }

        #endregion Constructor(s)

        [TestMethod]
        public void Test_Add_GetOk()
        {
            this.Test_Add_GetOk(
                this.KeyBuilder.UseKey(nameof(Test_Add_GetOk)),
                this.GetRedisDatabase(TimeSpan.FromDays(1)),
                expected: nameof(UnitTestMemoryCache)
            );
        }

        [TestMethod]
        public void Test_ItemExpired_ReturnNull()
        {
            var expiry = TimeSpan.FromMilliseconds(100);

            this.Test_ItemExpired_ReturnNull(
                this.KeyBuilder.UseKey(nameof(Test_ItemExpired_ReturnNull)),
                this.GetRedisDatabase(expiry),
                expiry
            );
        }

        [TestMethod]
        public void Test_ItemDelete_CannotGet()
        {
            this.Test_ItemDelete_CannotGet(
                this.KeyBuilder.UseKey(nameof(Test_ItemDelete_CannotGet)),
                this.GetRedisDatabase(TimeSpan.FromDays(1))
            );
        }

        [TestMethod]
        public void Test_GetOrAdd_Success()
        {
            this.Test_GetOrAdd_Success(
                this.KeyBuilder.UseKey(nameof(Test_GetOrAdd_Success)),
                this.GetRedisDatabase(TimeSpan.FromDays(1)),
                Guid.NewGuid().ToString()
            );
        }

        [TestMethod]
        public void Test_KeyWithSpace_GetOk()
        {
            this.Test_KeyWithSpace_GetOk(
                this.GetRedisDatabase(TimeSpan.FromDays(1))
            );
        }

        private Storage GetRedisDatabase(TimeSpan expiry)
        {
            return new Storage(
                this.redis.GetDatabase(),
                expiry
            );
        }
    }
}