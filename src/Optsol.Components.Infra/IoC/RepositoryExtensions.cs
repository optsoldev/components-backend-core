using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepository<TContext>(this IServiceCollection services, ContextOptionsBuilder options)
            where TContext: DbContext
        {
            services.AddDbContext<TContext>(options.Builder());
            services.AddScoped<DbContext, TContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(Repository<,>));

            return services;
        }

        public static IServiceCollection RegisterRepositories<TType>(this IServiceCollection services,  params string[] namespaces)
        {
            return services.RegisterScoped<TType>(namespaces);
        }
    }
}
