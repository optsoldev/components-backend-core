using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IRepository<TEntity, TKey> :
        IReadRepository<TEntity, TKey>,
        IWriteRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        DbContext Context { get; }
        DbSet<TEntity> Set { get; }
    }
}
