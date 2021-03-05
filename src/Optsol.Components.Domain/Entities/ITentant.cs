using System;

namespace Optsol.Components.Domain.Entities
{
    public interface ITenant<TKey>
    {
        TKey TenantId { get; }
    }

    public interface ITenant : ITenant<Guid> { }
}
