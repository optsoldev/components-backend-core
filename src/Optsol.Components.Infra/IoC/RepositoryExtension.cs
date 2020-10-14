
using System.Reflection;
using System;
using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.IoC;
using Optsol.Components.Infra.UoW;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepository<TContext>(this IServiceCollection services, ContextOptionsBuilder options)
            where TContext: DbContext
        {
            services.AddDbContext<TContext>(options.Builder());
            services.AddScoped<DbContext, TContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection RegisterRepositories<TType>(this IServiceCollection services,  params string[] namespaces)
        {
            var serviceTypes = Assembly.GetAssembly(typeof(TType)).GetTypes().Where(t => (t.IsInterface || t.IsAbstract) && (!namespaces.Any() || namespaces.Any(@namespace => t.Namespace.Contains(@namespace))));

            foreach (var serviceType in serviceTypes)
            {
                var implementationTypes = Assembly.GetAssembly(typeof(TType)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && (t.IsSubclassOf(serviceType) || t.GetInterfaces().Contains(serviceType)));
                foreach (var implementationType in implementationTypes)
                {
                    services.AddScoped(serviceType, implementationType);
                }
            }

            return services;
        }

    }
}
