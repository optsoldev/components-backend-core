using System;

namespace Microsoft.Extensions.DependencyInjection;

public interface ITenantProvider<out TKey>
{
    TKey TenantId { get; }
}

public interface ITenantProvider : ITenantProvider<Guid>
{
    
}