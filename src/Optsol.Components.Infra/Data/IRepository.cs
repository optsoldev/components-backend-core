using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IBaseRepository<TEntity, TKey> :
        IReadRepository<TEntity, TKey>,
        IMontoWriteRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {

    }

    public interface IRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        CoreContext Context { get; }

        DbSet<TEntity> Set { get; }
    }
}
