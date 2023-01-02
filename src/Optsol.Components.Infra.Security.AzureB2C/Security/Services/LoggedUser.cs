using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Securities;
using Optsol.Components.Infra.Data.Provider;

namespace Optsol.Components.Infra.Security.AzureB2C.Security.Services;

public class LoggedUser : ILoggedUser<Guid>, ITenantProvider
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
    private string GetClaimValue(string key)
    {
        return httpContextAccessor?.HttpContext?.User?.Identities
            ?.FirstOrDefault()?.Claims?.FirstOrDefault(x => x.Type == key)?.Value;
    }
}