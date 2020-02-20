using CodingCat.Cache.Redis.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodingCat.Cache.Redis.Extensions
{
    public static class IConnectionMultiplexerExtensions
    {
        public static IConnectionMultiplexer CreateRedisConnection(
            this ConfigurationOptions options,
            IConnectRedisConfiguration configuration,
            TextWriter log = null
        )
        {
            return options.CreateRedisConnection(
                configuration.TimeoutPerTry,
                configuration.RetryInterval,
                configuration.RetryUpTo,
                log
            );
        }

        public static IConnectionMultiplexer CreateRedisConnection(
            this ConfigurationOptions options,
            TimeSpan timeoutPerTry,
            TimeSpan retryInterval,
            uint retryUpTo,
            TextWriter log = null
        )
        {
            return CreateRedisConnection(
                () => ConnectionMultiplexer.Connect(options, log),
                timeoutPerTry,
                retryInterval,
                retryUpTo
            );
        }

        public static IConnectionMultiplexer CreateRedisConnection(
            this string connectionString,
            IConnectRedisConfiguration configuration,
            TextWriter log = null
        )
        {
            return connectionString.CreateRedisConnection(
                configuration.TimeoutPerTry,
                configuration.RetryInterval,
                configuration.RetryUpTo,
                log
            );
        }

        public static IConnectionMultiplexer CreateRedisConnection(
            this string connectionString,
            TimeSpan timeoutPerTry,
            TimeSpan retryInterval,
            uint retryUpTo,
            TextWriter log = null
        )
        {
            return CreateRedisConnection(
                () => ConnectionMultiplexer.Connect(connectionString, log),
                timeoutPerTry,
                retryInterval,
                retryUpTo
            );
        }

        public static IConnectionMultiplexer CreateRedisConnection(
            Func<IConnectionMultiplexer> factory,
            TimeSpan timeoutPerTry,
            TimeSpan retryInterval,
            uint retryUpTo
        )
        {
            for (var i = 0; i < retryUpTo; i++)
            {
                var connection = CreateRedisConnection(
                    factory,
                    timeoutPerTry
                );
                if (connection != null) return connection;
                if (i < retryUpTo) Thread.Sleep(retryInterval);
            }

            return null;
        }

        public static IConnectionMultiplexer CreateRedisConnection(
            Func<IConnectionMultiplexer> factory,
            TimeSpan timeout
        )
        {
            var connection = null as IConnectionMultiplexer;
            var connectedOrTimedOutEvent = new AutoResetEvent(false);

            Task.Delay(timeout)
                .ContinueWith(task => connectedOrTimedOutEvent.Set());
            Task.Run(() =>
            {
                connection = factory();

                if (connection != null)
                    if (!connection.IsConnected)
                        connection = null;
                connectedOrTimedOutEvent.Set();
            });

            connectedOrTimedOutEvent.WaitOne();
            return connection;
        }
    }
}
