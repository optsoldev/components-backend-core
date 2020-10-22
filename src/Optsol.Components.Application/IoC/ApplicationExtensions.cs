using Optsol.Components.Application.Result;
using Optsol.Components.Application.Service;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationExtensions
    {   
        public static IServiceCollection AddApplicationServices<TType>(this IServiceCollection services,  params string[] namespaces)
        {
            services.RegisterScoped<TType>(namespaces);
            services.AddScoped(typeof(IBaseServiceApplication<,,,,>), typeof(BaseServiceApplication<,,,,>));
            services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

            return services;
        } 
    }
}
