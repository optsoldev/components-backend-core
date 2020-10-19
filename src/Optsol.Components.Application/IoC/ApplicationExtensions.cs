using Optsol.Components.Application.Result;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationExtensions
    {   
        public static IServiceCollection AddApplicationServices<TType>(this IServiceCollection services,  params string[] namespaces)
        {
            services.RegisterScoped<TType>(namespaces);
            services.AddTransient<IServiceResultFactory, ServiceResultFactory>();

            return services;
        } 
    }
}
