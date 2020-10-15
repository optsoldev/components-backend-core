using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection RegisterApplicationServices<TType>(this IServiceCollection services,  params string[] namespaces)
        {
            return services.RegisterScoped<TType>(namespaces);
        } 
    }
}
