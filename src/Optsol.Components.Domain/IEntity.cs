using System;

namespace Optsol.Components.Domain
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
        DateTime CreateDate { get; }
    }
}
