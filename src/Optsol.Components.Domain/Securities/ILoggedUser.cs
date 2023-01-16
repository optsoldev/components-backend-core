using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public interface ILoggedUser<out TKey> : ITenantProvider<TKey>
{
    TKey ApplicationId { get; }
    string Username { get; }
    TKey UserExternalId { get; }
    string[] Claims { get; }
    string GetClaim(string key);
    string Token { get; }
}

public interface ILoggedUser : ILoggedUser<Guid>
{
    
}