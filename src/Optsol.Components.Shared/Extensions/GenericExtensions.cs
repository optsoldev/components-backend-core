using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Optsol.Components.Shared.Resolvers;
using System.Linq;

namespace System
{
    public static class GenericExtensions
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
                settings.ContractResolver = new IgnorePropertiesResolver(ignoredProps);
            }

            return JsonConvert.SerializeObject(source, Formatting.Indented, settings);
        }
    }
}
