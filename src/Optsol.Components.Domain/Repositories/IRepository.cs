using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Domain.Data
{
    public interface IRepository<TEntity, TKey> :
        IReadRepository<TEntity, TKey>,
        IWriteRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {

    }
}
