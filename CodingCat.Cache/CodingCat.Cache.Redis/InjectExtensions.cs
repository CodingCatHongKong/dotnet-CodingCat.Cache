using CodingCat.Cache.Redis.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodingCat.Cache.Redis
{
    public static class InjectExtensions
    {
        public static IServiceCollection AddRedisStorageConfig<T>(
            this IServiceCollection services,
            T configuration
        ) where T : class, IRedisStorageConfiguration
        {
            return services
                .AddSingleton<IRedisStorageConfiguration>(configuration);
        }
    }
}