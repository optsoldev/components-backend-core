using Optsol.Components.Domain.Data;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using System;
using System.Collections;
using Optsol.Components.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddContext<TContext>(this IServiceCollection services, Action<RepositoryOptions> options = null)
            where TContext : CoreContext
        {
            var repositoryOptions = new RepositoryOptions(services);
            options?.Invoke(repositoryOptions);

            services.AddDbContext<TContext>(repositoryOptions.Builder());

            services.AddScoped<CoreContext, TContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(Repository<,>));

            return services;
        }       
    }
}
