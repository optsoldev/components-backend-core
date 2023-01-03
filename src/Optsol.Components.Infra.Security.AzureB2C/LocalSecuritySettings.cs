using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class LocalSecuritySettings
{
    public readonly static string Issuer = "issuer";
    public readonly static string Audience = "audience";
    public readonly static string Key = "optsol-security-key";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}