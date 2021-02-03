using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Domain.Notifications;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainNotifications(this IServiceCollection services)
        {
            services.AddScoped<NotificationContext>();

            return services;
        }

        public static IServiceCollection AddDomainService<TInterface, TImplementation>(this IServiceCollection services, params string[] namespaces)
        {
            services.RegisterScoped<TInterface, TImplementation>(namespaces);

            return services;
        }
    }
}
