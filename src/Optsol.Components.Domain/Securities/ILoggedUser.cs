using System;

namespace Microsoft.Extensions.DependencyInjection.Securities;

public interface ILoggedUser
{
    Guid GetApplicationId();
    string GetUsername();
    Guid GetUserExternalId();
    Guid GetTenantId();
    string[] GetClaims();
    string GetClaim(string key);
    void SetToken(string jwtToken);
    string GetToken();
}