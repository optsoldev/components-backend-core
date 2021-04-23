using System;

namespace Optsol.Components.Domain.Entities
{
    public interface ITenant<TKey>
    {
        TKey TenantId { get; }

        public void SetTenantId(TKey tenantId);
    }

    public interface ITenant : ITenant<Guid> { }
}
