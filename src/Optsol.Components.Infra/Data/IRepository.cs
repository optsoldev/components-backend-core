using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain;

namespace Optsol.Components.Infra.Data
{
    //Todo: Mover para camada de Domain

    public interface IRepository<TEntity, TKey> :
        IReadRepository<TEntity, TKey>,
        IWriteRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        DbContext Context { get; }
        DbSet<TEntity> Set { get; }
    }
}
