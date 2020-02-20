using CodingCat.Cache.Redis.Extensions;
using CodingCat.Cache.Redis.Interfaces;
using CodingCat.Cache.Tests.Impls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System;

namespace CodingCat.Cache.Tests
{
    [TestClass]
    public class TestRedisConnect
    {
        public static readonly IConnectRedisConfiguration ConnectRedisConfiguration = new ConnectRedisConfiguration()
        {
            TimeoutPerTry = TimeSpan.FromSeconds(2),
            RetryInterval = TimeSpan.FromSeconds(1),
            RetryUpTo = 3
        };

        [TestMethod]
        public void Test_ConnectInvalidServer_WillRetry()
        {
            const string USING_SERVER = "127.0.0.1:1234";

            // Arrange
            uint actual = 0;
            var expected = ConnectRedisConfiguration.RetryUpTo;

            // Act
            IConnectionMultiplexerExtensions
                .CreateRedisConnection(
                    () =>
                    {
                        actual += 1;
                        return ConnectionMultiplexer.Connect(USING_SERVER);
                    },
                    ConnectRedisConfiguration.TimeoutPerTry,
                    ConnectRedisConfiguration.RetryInterval,
                    ConnectRedisConfiguration.RetryUpTo
                );

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_ConnectInvalidServer_ReturnNull()
        {
            const string USING_SERVER = "127.0.0.1:1234";

            // Arrange

            // Act
            var actual = USING_SERVER.CreateRedisConnection(
                ConnectRedisConfiguration
            );

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Test_ConnectValidServer_Ok()
        {
            // Arrange

            // Act
            var actual = Constants.REDIS_CONFIG
                .CreateRedisConnection(ConnectRedisConfiguration);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsConnected);
        }
    }
}