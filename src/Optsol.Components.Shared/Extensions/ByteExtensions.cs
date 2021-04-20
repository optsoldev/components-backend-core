using System.Text;

namespace System
{
    public static class ByteExtensions
    {
        public static string ToText(this byte[] source)
        {
            return Encoding.UTF8.GetString(source);
        }
    }
}
