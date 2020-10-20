using Optsol.Components.Service.Response;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAServices(this IServiceCollection services)
        {
            services.AddTransient<IResponseFactory, ResponseFactory>();

            return services;
        } 
    }
}
