using System.Text.RegularExpressions;

namespace Optsol.Components.Shared.Extensions
{
    public static class RegexExtensions
    {
        public static bool IsValidUrl(this string source)
        {
            var rgx = new Regex(@"^(http|ftp|https|www)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$", RegexOptions.IgnoreCase);

            return rgx.IsMatch(source);
        }

        public static bool IsValidEndpoint(this string source)
        {
            var rgx = new Regex(@"\/$", RegexOptions.IgnoreCase);

            return IsValidUrl(source) && !rgx.IsMatch(source);
        }
    }
}
