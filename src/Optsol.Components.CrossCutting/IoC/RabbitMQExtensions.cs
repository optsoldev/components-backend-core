using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Bus.Services;
using Optsol.Components.Infra.RabbitMQ.Connections;
using Optsol.Components.Infra.RabbitMQ.Services;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var servicesProvider = services.BuildServiceProvider();

            var rabbitSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>()
                ?? throw new RabbitMQSettingsNullException(servicesProvider.GetRequiredService<ILoggerFactory>());

            services.AddSingleton(rabbitSettings);
            services.AddScoped<IRabbitMQConnection, RabbitMQConnection>();
            services.AddScoped<IEventBus, EventBusRabbitMQ>();

            return services;
        }
    }
}
