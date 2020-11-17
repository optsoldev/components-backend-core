using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationExtensions
    {   
        public static IServiceCollection AddApplicationServices<TInterface, TImplementation>(this IServiceCollection services,  params string[] namespaces)
        {
            services.RegisterScoped<TInterface, TImplementation>(namespaces);
            services.AddScoped(typeof(IBaseServiceApplication<,,,,>), typeof(BaseServiceApplication<,,,,>));
            services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

            return services;
        } 
    }
}
