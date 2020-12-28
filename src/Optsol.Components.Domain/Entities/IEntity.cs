using System;

namespace Optsol.Components.Domain.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
        DateTime CreatedDate { get; }
        void Validate();
    }
}
