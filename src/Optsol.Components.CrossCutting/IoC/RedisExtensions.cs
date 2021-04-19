using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Redis.Connections;
using Optsol.Components.Infra.Redis.Services;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var servicesProvider = services.BuildServiceProvider();

            var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>()
                ?? throw new RedisSettingsNullException(servicesProvider.GetRequiredService<ILoggerFactory>());

            services.AddSingleton(redisSettings);

            services.AddSingleton<RedisCacheConnection>();

            services.AddScoped<IRedisCacheService, RedisCacheService>();

            return services;
        }
    }
}
