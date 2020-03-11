using CodingCat.Cache.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingCat.Cache
{
    public static class InjectExtensions
    {
        public static IServiceCollection AddStorage<T>(
            this IServiceCollection services
        ) where T : class, IStorage
        {
            return services
                .AddTransient<IStorage, T>()
                .AddTransient<T, T>();
        }

        public static IServiceCollection AddStorageManager<T>(
            this IServiceCollection services
        ) where T : class, IStorageManager
        {
            return services
                .AddSingleton<Func<IStorage, IStorageManager>>(
                    provider => defaultStorage =>
                    {
                        var fallbacks = provider
                            .Require<IEnumerable<IStorage>>()
                            .Where(fallback =>
                                fallback.GetType() != defaultStorage.GetType()
                            );

                        var manager = ActivatorUtilities
                            .CreateInstance<T>(
                                provider,
                                defaultStorage
                            );

                        foreach (var fallback in fallbacks)
                            manager.AddFallback(fallback);

                        return manager;
                    }
                );
        }

        public static IServiceCollection AddStorageConfig<T>(
            this IServiceCollection services,
            T configuration
        ) where T : class, IStorageConfiguration
        {
            return services
                .AddSingleton<IStorageConfiguration>(configuration);
        }
    }
}