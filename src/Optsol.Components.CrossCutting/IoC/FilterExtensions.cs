using Newtonsoft.Json.Serialization;
using Optsol.Components.Service.Resolver;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FilterExtensions
    {
        public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new IgnorePropertiesResolver(new[] { "notifications", "invalid", "valid" })
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            return builder;
        }
    }
}
