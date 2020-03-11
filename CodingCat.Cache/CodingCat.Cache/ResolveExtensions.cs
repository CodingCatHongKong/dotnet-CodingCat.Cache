using CodingCat.Cache.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CodingCat.Cache
{
    public static class ResolveExtensions
    {
        public static IStorageManager ResolveStorageManager(
            this IServiceProvider provider,
            IStorage defaultStorage
        )
        {
            return provider
                .Require<Func<IStorage, IStorageManager>>()(
                    defaultStorage
                );
        }

        public static IStorageManager ResolveStorageManager<T>(
            this IServiceProvider provider
        ) where T : class, IStorage
        {
            return provider.ResolveStorageManager(
                provider.Require<T>()
            );
        }

        internal static T Require<T>(this IServiceProvider provider) => provider.GetRequiredService<T>();
    }
}