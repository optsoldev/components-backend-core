using System;

namespace Microsoft.Extensions.DependencyInjection.Securities;

public interface ILoggedUser<out TKey>
{
    TKey GetApplicationId();
    string GetUsername();
    TKey GetUserExternalId();
    TKey GetTenantId();
    string[] GetClaims();
    string GetClaim(string key);
}