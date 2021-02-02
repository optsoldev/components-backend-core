using Microsoft.Extensions.Configuration;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
            mongoSettings.Validate();



            return services;
        }
    }
}
