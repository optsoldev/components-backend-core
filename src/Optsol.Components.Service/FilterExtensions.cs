using Newtonsoft.Json.Serialization;
using Optsol.Components.Shared.Resolvers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FilterExtensions
    {
        private static readonly string[] IgnoreProperties = { "notifications", "invalid", "valid" };

        public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new IgnorePropertiesResolver(IgnoreProperties)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            return builder;
        }
    }
}
