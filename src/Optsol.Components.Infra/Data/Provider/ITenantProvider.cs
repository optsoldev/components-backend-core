using System;

namespace Optsol.Components.Infra.Data.Provider
{
    public interface ITenantProvider<TKey>
    {
        TKey GetTenantId();
    }

    public interface ITenantProvider : ITenantProvider<Guid>
    {

    }
}
