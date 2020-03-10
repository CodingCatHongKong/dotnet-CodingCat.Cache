using CodingCat.Cache.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingCat.Cache
{
    public static class InjectExtensions
    {
        public static IServiceCollection AddKeyBuilder<T>(
            this IServiceCollection services,
            IKeyBuilderConfiguration configuration
        ) where T : class, IKeyBuilder
        {
            return services
                .AddTransient(provider => configuration)
                .AddTransient<IKeyBuilder, T>();
        }

        public static IServiceCollection AddKeyBuilder<T>(
            this IServiceCollection services
        ) where T : class, IKeyBuilder
        {
            return services
                .AddTransient<IKeyBuilder, T>()
                .AddSingleton<Func<IKeyBuilderConfiguration, IKeyBuilder>>(
                    provider => config =>
                        ActivatorUtilities.CreateInstance<IKeyBuilder>(
                            provider,
                            config
                        )
                );
        }

        public static IServiceCollection AddStorageManager<T>(
            this IServiceCollection services
        ) where T : class, IStorageManager
        {
            return services
                .AddTransient<IStorageManager, T>()
                .AddSingleton<Func<IStorage, IStorageManager>>(
                    provider => defaultStorage =>
                    {
                        var fallbacks = provider
                            .GetService<IEnumerable<IStorage>>()
                            .Where(fallback =>
                                fallback.GetType() != defaultStorage.GetType()
                            );

                        var manager = ActivatorUtilities
                            .CreateInstance<IStorageManager>(
                                provider,
                                defaultStorage
                            );

                        foreach (var fallback in fallbacks)
                            manager.AddFallback(fallback);

                        return manager;
                    }
                );
        }
    }
}