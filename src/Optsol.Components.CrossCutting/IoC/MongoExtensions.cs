using Microsoft.Extensions.Configuration;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Infra.MongoDB.Repository;
using Optsol.Components.Infra.MongoDB.UoW;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongoContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : MongoContext
        {
            var mongoSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
            mongoSettings.Validate();

            services.AddSingleton(mongoSettings);
            services.AddScoped<MongoContext>();
            services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
            services.AddScoped(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(MongoRepository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(MongoRepository<,>));

            return services;
        }

        public static IServiceCollection AddMongoRepository<TInterface, TImplementation>(this IServiceCollection services, params string[] namespaces)
        {
            return services.RegisterScoped<TInterface, TImplementation>(namespaces);
        }
    }
}
