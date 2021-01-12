using System;

namespace Optsol.Components.Domain.Entities
{
    public interface IEntity
    {
        DateTime CreatedDate { get; }
        void Validate();
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}
