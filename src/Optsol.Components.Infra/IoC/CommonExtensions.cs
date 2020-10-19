using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommonExtensions
    {
        public static IServiceCollection RegisterScoped<TType>(this IServiceCollection services, string[] namespaces)
        {
            return services.Register<TType>(namespaces, services.AddScoped);
        }

        public static IServiceCollection RegisterTransinente<TType>(this IServiceCollection services, string[] namespaces)
        {
            return services.Register<TType>(namespaces, services.AddTransient);
        }

        public static IServiceCollection RegisterSingleton<TType>(this IServiceCollection services, string[] namespaces)
        {
            return services.Register<TType>(namespaces, services.AddSingleton);
        }

        private static IServiceCollection Register<TType>(this IServiceCollection services, string[] namespaces, Func<Type, Type, IServiceCollection> addService)
        {
            var serviceTypes = Assembly
                .GetAssembly(typeof(TType))
                .GetTypes()
                .Where(t => (t.IsInterface || t.IsAbstract) && (!namespaces.Any() || namespaces.Any(@namespace => t.Namespace.Contains(@namespace))));

            foreach (var serviceType in serviceTypes)
            {
                var implementationTypes = Assembly
                    .GetAssembly(typeof(TType))
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && (t.IsSubclassOf(serviceType) || t.GetInterfaces().Contains(serviceType)));

                foreach (var implementationType in implementationTypes)
                {
                    addService(serviceType, implementationType);
                }
            }

            return services;
        }
    }
}
