using System;

namespace Optsol.Components.Domain.Entities
{
    public interface IEntity
    {
        DateTime CreatedDate { get; }
        void Validate();
    }

    public interface IEntity<out TKey> : IEntity
    {
        TKey Id { get; }
    }
}
