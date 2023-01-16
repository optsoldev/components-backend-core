using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Optsol.Components.Infra.Security.AzureB2C.Security.Services;

public class LoggedUser : ILoggedUser, ITenantProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public LoggedUser(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
    public Guid ApplicationId
        => new Guid(GetClaimValue("azp"));
    public string Username
        => GetClaimValue("name");
    public Guid UserExternalId
        => new Guid(GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
    public Guid TenantId
    {
        get
        {
            var claimValue = GetClaimValue("extension_TenantClaim");
            return string.IsNullOrWhiteSpace(claimValue) ? default : new Guid(claimValue);
        }
    }
    public string[] Claims => GetClaimValue("extension_SecurityClaim").Split(";");
    public string GetClaim(string key) => GetClaimValue(key);

    public string? Token
    {
        get
        {
            var authorizationHeader =
                httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
            string? token = null;

            if (authorizationHeader is null) return token;
            
            var tokenHeader = AuthenticationHeaderValue.Parse(authorizationHeader);
            token = tokenHeader.Parameter;

            return token;
        }
    }

    private string GetClaimValue(string key)
    {
        return httpContextAccessor.HttpContext?.User?.Identities
            ?.FirstOrDefault()?.Claims?.FirstOrDefault(x => x.Type == key)?.Value;
    }
}