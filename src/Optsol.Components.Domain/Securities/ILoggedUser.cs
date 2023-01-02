using System;

namespace Microsoft.Extensions.DependencyInjection.Securities;

public interface ILoggedUser<out TKey>
{
    TKey ApplicationId { get; }
    string Username { get; }
    TKey UserExternalId { get; }
    string[] Claims { get; }
    string GetClaim(string key);
}

public interface ILoggedUser : ILoggedUser<Guid>
{
    
}