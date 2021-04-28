using System;

namespace Optsol.Components.Infra.Data.Provider
{
    public interface ITenantProvider<out TKey>
    {
        TKey GetTenantId();
    }

    public interface ITenantProvider : ITenantProvider<Guid>
    {

    }
}
