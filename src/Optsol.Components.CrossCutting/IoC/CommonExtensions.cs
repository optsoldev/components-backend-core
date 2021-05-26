using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommonExtensions
    {
        public static IServiceCollection RegisterScoped<TInterface, TImplementation>(this IServiceCollection services, string[] namespaces)
        {
            return services.Register<TInterface, TImplementation>(namespaces, services.AddScoped);
        }

        public static IServiceCollection RegisterTransient<TInterface, TImplementation>(this IServiceCollection services, string[] namespaces)
        {
            return services.Register<TInterface, TImplementation>(namespaces, services.AddTransient);
        }

        public static IServiceCollection RegisterSingleton<TInterface, TImplementation>(this IServiceCollection services, string[] namespaces)
        {
            return services.Register<TInterface, TImplementation>(namespaces, services.AddSingleton);
        }

        private static IServiceCollection Register<TInterface, TImplementation>(this IServiceCollection services, string[] namespaces, Func<Type, Type, IServiceCollection> addService)
        {
            var serviceTypes = Assembly
                .GetAssembly(typeof(TInterface))
                .GetTypes()
                .Where(t => (t.IsInterface || t.IsAbstract) && (!namespaces.Any() || namespaces.Any(@namespace => t.Namespace.Contains(@namespace))));

            foreach (var serviceType in serviceTypes)
            {
                var implementationTypes = Assembly
                    .GetAssembly(typeof(TImplementation))
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
