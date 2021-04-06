using Newtonsoft.Json;

namespace Optsol.Components.Shared.Extensions
{
    public static class StringExtensions
    {
        public static T To<T>(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(source);
        }
    }
}
