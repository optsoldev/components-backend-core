using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Optsol.Components.Service.Filters;
using Optsol.Components.Service.Responses;
using Optsol.Components.Service.Transformers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddControllers(option => option.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())))
                .ConfigureNewtonsoftJson();

            services.AddScoped<ValidationModelAttribute>();
            services.AddTransient<IResponseFactory, ResponseFactory>();

            return services;
        }
    }
}
