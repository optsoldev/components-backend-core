using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Domain.Repositories;

public interface IBaseRepository<TEntity,TKey> :
    IReadBaseRepository<TEntity, TKey>,
    IWriteBaseRepository<TEntity, TKey>
    where TEntity : class, IAggregateRoot<TKey>;