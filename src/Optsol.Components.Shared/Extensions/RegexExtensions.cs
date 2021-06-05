using System.Text.RegularExpressions;

namespace Optsol.Components.Shared.Extensions
{
    public static class RegexExtensions
    {
        public static bool IsUrlValid(this string source)
        {
            var rgx = new Regex(@"^(http|ftp|https|www)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$", RegexOptions.IgnoreCase);

            return rgx.IsMatch(source);
        }
    }
}
