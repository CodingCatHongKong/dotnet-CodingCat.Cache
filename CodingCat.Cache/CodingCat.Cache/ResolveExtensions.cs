using CodingCat.Cache.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CodingCat.Cache
{
    public static class ResolveExtensions
    {
        public static IKeyBuilder Resolve(this IServiceProvider provider)
        {
            return provider.GetService<IKeyBuilder>();
        }

        public static IKeyBuilder Require(this IServiceProvider provider)
        {
            return provider.GetRequiredService<IKeyBuilder>();
        }

        public static IKeyBuilder Resolve(
            this IServiceProvider provider,
            IKeyBuilderConfiguration configuration
        )
        {
            return provider
                .GetService<Func<IKeyBuilderConfiguration, IKeyBuilder>>()(
                    configuration
                );
        }

        public static IStorageManager Resolve(
            this IServiceProvider provider,
            IStorage defaultStorage
        )
        {
            return provider
                .GetService<Func<IStorage, IStorageManager>>()(
                    defaultStorage
                );
        }
    }
}