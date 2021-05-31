using System;

namespace Optsol.Components.Domain.Entities
{
    public interface ICreationUser<TKey>
    {
        TKey CreationUserId { get; }

        public void SetCreationUserId(TKey creationUserId);
    }

    public interface ICreationUser : ICreationUser<Guid> { }
}
