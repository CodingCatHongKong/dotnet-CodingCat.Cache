using CodingCat.Cache.Enums;
using CodingCat.Cache.Impls;
using CodingCat.Cache.Tests.Abstracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System;
using MemoryStorage = CodingCat.Cache.Memory.Storage;
using RedisStorage = CodingCat.Cache.Redis.Storage;

namespace CodingCat.Cache.Tests
{
    [TestClass]
    public class UnitTestStorageManager : BaseCachingTest<UnitTestStorageManager>
    {
        private IConnectionMultiplexer redis { get; }

        #region Constructor(s)

        public UnitTestStorageManager()
        {
            this.redis = ConnectionMultiplexer.Connect(
                Constants.REDIS_CONFIG
            );
        }

        #endregion Constructor(s)

        [TestMethod]
        public void Test_Add_GetOk()
        {
            var memoryStorage = new MemoryStorage(TimeSpan.FromSeconds(10));
            var redisStorage = this.GetRedisStorage(TimeSpan.FromSeconds(10));

            this.Test_Add_GetOk(
                this.KeyBuilder.UseKey(nameof(Test_Add_GetOk)),
                new StorageManager()
                    .SetDefault(memoryStorage)
                    .AddFallback(redisStorage),
                Guid.NewGuid().ToString()
            );
        }

        [TestMethod]
        public void Test_ItemExpired_ReturnNull()
        {
            var expiry = TimeSpan.FromMilliseconds(100);
            var memoryStorage = new MemoryStorage(expiry);
            var redisStorage = this.GetRedisStorage(expiry);

            this.Test_ItemExpired_ReturnNull(
                this.KeyBuilder.UseKey(nameof(Test_ItemExpired_ReturnNull)),
                new StorageManager()
                    .SetDefault(memoryStorage)
                    .AddFallback(redisStorage),
                expiry
            );
        }

        [TestMethod]
        public void Test_ItemDelete_CannotGet()
        {
            var memoryStorage = new MemoryStorage(TimeSpan.FromSeconds(10));
            var redisStorage = this.GetRedisStorage(TimeSpan.FromSeconds(10));

            this.Test_ItemDelete_CannotGet(
                this.KeyBuilder.UseKey(nameof(Test_ItemDelete_CannotGet)),
                new StorageManager()
                    .SetDefault(memoryStorage)
                    .AddFallback(redisStorage)
            );
        }

        [TestMethod]
        public void Test_GetOrAdd_Success()
        {
            var memoryStorage = new MemoryStorage(TimeSpan.FromSeconds(10));
            var redisStorage = this.GetRedisStorage(TimeSpan.FromSeconds(10));

            this.Test_GetOrAdd_Success(
                this.KeyBuilder.UseKey(nameof(Test_GetOrAdd_Success)),
                new StorageManager()
                    .SetDefault(memoryStorage)
                    .AddFallback(redisStorage),
                Guid.NewGuid().ToString()
            );
        }

        [TestMethod]
        public void Test_GetFromFallback_Ok_NotSaved()
        {
            // Arrange
            var usingKey = this.KeyBuilder.UseKey(
                nameof(Test_GetFromFallback_Ok_NotSaved)
            );
            var expected = Guid.NewGuid().ToString();
            var expiry = TimeSpan.FromMinutes(1);

            var memoryStorage = new MemoryStorage(expiry);
            var redisStorage = this.GetRedisStorage(expiry)
                .Delete(usingKey)
                .Add(usingKey, expected);
            var cacheManager = new StorageManager()
                .SetDefault(memoryStorage)
                .AddFallback(redisStorage);

            // Actual
            var actual = cacheManager.Get(usingKey);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.IsNull(cacheManager.DefaultStorage.Get(usingKey));
        }

        [TestMethod]
        public void Test_GetFromFallback_Ok_Saved()
        {
            // Arrange
            var usingKey = this.KeyBuilder.UseKey(
                nameof(Test_GetFromFallback_Ok_Saved)
            );
            var expected = Guid.NewGuid().ToString();
            var expiry = TimeSpan.FromMinutes(1);

            var memoryStorage = new MemoryStorage(expiry);
            var redisStorage = this.GetRedisStorage(expiry)
                .Delete(usingKey)
                .Add(usingKey, expected);
            var cacheManager = new StorageManager(FallbackPolicy.SaveFromFallback)
                .SetDefault(memoryStorage)
                .AddFallback(redisStorage);

            // Actual
            var actual = cacheManager.Get(usingKey);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(
                expected,
                cacheManager.DefaultStorage.Get(usingKey)
            );
        }

        [TestMethod]
        public void Test_GetFromFallback_OverridePolicy_Ok_Saved()
        {
            // Arrange
            var usingKey = this.KeyBuilder.UseKey(
                nameof(Test_GetFromFallback_OverridePolicy_Ok_Saved)
            );
            var expected = Guid.NewGuid().ToString();
            var expiry = TimeSpan.FromMinutes(1);

            var memoryStorage = new MemoryStorage(expiry);
            var redisStorage = this.GetRedisStorage(expiry)
                .Delete(usingKey)
                .Add(usingKey, expected);
            var cacheManager = new StorageManager()
                .SetDefault(memoryStorage)
                .AddFallback(redisStorage);

            // Actual
            cacheManager.Get(usingKey, FallbackPolicy.SaveFromFallback);
            var actual = memoryStorage.Get(usingKey);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetFromFallback_OverridePolicy_Ok_NotSaved()
        {
            // Arrange
            var usingKey = this.KeyBuilder.UseKey(
                nameof(Test_GetFromFallback_OverridePolicy_Ok_NotSaved)
            );
            var expected = Guid.NewGuid().ToString();
            var expiry = TimeSpan.FromMinutes(1);

            var memoryStorage = new MemoryStorage(expiry);
            var redisStorage = this.GetRedisStorage(expiry)
                .Delete(usingKey)
                .Add(usingKey, expected);
            var cacheManager = new StorageManager(FallbackPolicy.SaveFromFallback)
                .SetDefault(memoryStorage)
                .AddFallback(redisStorage);

            // Actual
            cacheManager.Get(usingKey, FallbackPolicy.Default);
            var actual = memoryStorage.Get(usingKey);

            // Assert
            Assert.IsNull(actual);
        }

        private RedisStorage GetRedisStorage(TimeSpan expiry)
        {
            return new RedisStorage(
                this.redis.GetDatabase(),
                expiry
            );
        }
    }
}