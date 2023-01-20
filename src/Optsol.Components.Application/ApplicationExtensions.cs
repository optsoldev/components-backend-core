using AutoMapper;
using Optsol.Components.Application.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ApplicationOptions
    {
        private readonly IServiceCollection services;

        public ApplicationOptions(IServiceCollection services)
        {
            this.services = services;
        }

        public ApplicationOptions ConfigureAutoMapper<TMapper>()
            where TMapper : Profile
        {
            services.AddAutoMapper(typeof(TMapper));

            return this;
        }

        public ApplicationOptions ConfigureServices<TInterface, TImplementation>(params string[] namespaces)
        {
            services.RegisterScoped<TInterface, TImplementation>(namespaces);

            return this;
        }
    }

    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplications(this IServiceCollection services, Action<ApplicationOptions> options)
        {
            var applicationOptions = new ApplicationOptions(services);
            options?.Invoke(applicationOptions);
            
            services.AddScoped(typeof(IBaseServiceApplication<>), typeof(BaseServiceApplication<>));
            return services;
        }
        
        public static IServiceCollection AddValidations(this IServiceCollection services, Action<ApplicationOptions> options)
        {
            var applicationOptions = new ApplicationOptions(services);
            options?.Invoke(applicationOptions);
            
            return services;
        }
    }
}
