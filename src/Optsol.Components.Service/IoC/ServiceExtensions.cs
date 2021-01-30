using Optsol.Components.Service;
using Optsol.Components.Service.Filters;
using Optsol.Components.Service.Response;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAServices(this IServiceCollection services)
        {
            services.AddScoped<ValidationModelAttribute>();
            services.AddTransient<IResponseFactory, ResponseFactory>();

            return services;
        }
    }
}
