using System;

namespace Optsol.Components.Infra.Data.Provider
{
    public interface ITenantProvider<out TKey>
    {
        TKey TenantId { get; }
    }

    /// <inheritdoc />
    public interface ITenantProvider : ITenantProvider<Guid>
    {

    }
}
