using Microsoft.Extensions.Configuration;
using Optsol.Components.Domain.Data;
using Optsol.Components.Infra.ElasticSearch.Context;
using Optsol.Components.Infra.ElasticSearch.Repositories;
using Optsol.Components.Infra.ElasticSearch.UoW;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ElasticExtensions
    {
        public static IServiceCollection AddElasticContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : ElasticContext
        {
            var mongoSettings = configuration.GetSection(nameof(ElasticSearchSettings)).Get<ElasticSearchSettings>();
            mongoSettings.Validate();

            services.AddSingleton(mongoSettings);
            services.AddScoped<ElasticContext>();
            services.AddScoped<TContext>();
            services.AddScoped<IElasticUnitOfWork, ElasticUnitOfWork>();
            services.AddScoped(typeof(IElasticRepository<,>), typeof(ElasticRepository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(ElasticRepository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(ElasticRepository<,>));

            return services;
        }

        public static IServiceCollection AddElasticRepository<TInterface, TImplementation>(this IServiceCollection services, params string[] namespaces)
        {
            return services.RegisterScoped<TInterface, TImplementation>(namespaces);
        }
    }
}
