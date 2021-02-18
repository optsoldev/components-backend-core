using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddContext<TContext>(this IServiceCollection services, ContextOptionsBuilder options)
            where TContext: CoreContext
        {         
            services.AddDbContext<TContext>(options.Builder());
            services.AddScoped<CoreContext, TContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(Repository<,>));

            return services;
        }

        public static IServiceCollection AddRepository<TInterface, TImplementation>(this IServiceCollection services,  params string[] namespaces)
        {
            return services.RegisterScoped<TInterface, TImplementation>(namespaces);
        }
    }
}
