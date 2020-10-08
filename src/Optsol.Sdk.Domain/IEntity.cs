using System;

namespace Optsol.Sdk.Domain
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
        DateTime CreateDate { get; }
    }
}
