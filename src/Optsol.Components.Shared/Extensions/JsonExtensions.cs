using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Optsol.Components.Shared.Resolvers;
using System.Linq;

namespace Optsol.Components.Shared.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T source, params string[] ignoredProps)
        {
            if (source == null) return "{}";

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            if (ignoredProps.Any())
            {
                settings.ContractResolver = new IgnorePropertiesResolver(ignoredProps)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            }

            return JsonConvert.SerializeObject(source, Formatting.Indented, settings);
        }

    }
}
