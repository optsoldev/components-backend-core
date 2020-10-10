using Newtonsoft.Json;

namespace Optsol.Sdk.Shared.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T source)
        {
            if(source == null) return "{}";

            return JsonConvert.SerializeObject(source);
        }
    }
}
