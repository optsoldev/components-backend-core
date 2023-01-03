using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    private static Claim[] GetLocalClaims(SecuritySettings settings)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, "Optosol Local User"),
            new Claim(ClaimTypes.Email, "optsol.local@optsol.com.br"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim("nonce", "635851137282360087.M2Y2NDM5NmUtMmRkYS00YWVjLTgwMWEtYWYwNDQyYzFmZmIxOTk0YjBlNmQtNjE5ZC00MDg5LWE2NTItNjc0OTJjNTU1Y2Rm"),
            new Claim("iat", "1449516934"),
            new Claim("at_hash", "vGWPYD4ZcS5RbJh6kPtwOw"),
            new Claim("http://schemas.microsoft.com/claims/authnmethodsreferences", "password"),
            new Claim("auth_time", "1449516934"),
            new Claim("http://schemas.microsoft.com/identity/claims/identityprovider", "devtest"),
            new Claim(  "extension_SecurityClaim", string.Join(';',settings.DevelopmentClaims))
        };
    }

    public static IApplicationBuilder UseRemoteEndpoint(this IApplicationBuilder app, SecuritySettings settings)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/token", async (context) =>
            {
                var symmetricSecurityKey = LocalSecuritySettings.GetSymmetricSecurityKey();
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    LocalSecuritySettings.Issuer,
                    LocalSecuritySettings.Audience,
                    GetLocalClaims(settings),
                    DateTime.Now,
                    DateTime.UtcNow.AddYears(1),
                    signingCredentials);

                await context.Response.WriteAsync($"Token: { new JwtSecurityTokenHandler().WriteToken(token) }");
            });
        });

        return app;
    }
}