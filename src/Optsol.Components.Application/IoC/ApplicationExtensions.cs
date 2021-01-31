using Optsol.Components.Application.Results;
using Optsol.Components.Application.Services;
using Optsol.Components.Domain.Notifications;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationExtensions
    {   
        public static IServiceCollection AddApplicationServices<TInterface, TImplementation>(this IServiceCollection services,  params string[] namespaces)
        {
            services.RegisterScoped<TInterface, TImplementation>(namespaces);
            services.AddScoped(typeof(IBaseServiceApplication<,,,,>), typeof(BaseServiceApplication<,,,,>));
            services.AddTransient<IServiceResultFactory, ServiceResultFactory>();
            
            services.AddScoped<NotificationContext>();

            return services;
        } 
    }
}
