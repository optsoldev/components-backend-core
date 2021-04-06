using System.Text;

namespace Optsol.Components.Shared.Extensions
{
    public static class ByteExtensions
    {
        public static byte[] ToBytes(this string source)
        {
            return Encoding.UTF8.GetBytes(source);
        }

        public static string ToText(this byte[] source)
        {
            return Encoding.UTF8.GetString(source);
        }
    }
}
