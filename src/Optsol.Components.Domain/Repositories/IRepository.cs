using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Repositories;

namespace Optsol.Components.Domain.Data
{
    public interface IRepository<TEntity, TKey> :
        IReadRepository<TEntity, TKey>,
        IWriteRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {

    }
}
