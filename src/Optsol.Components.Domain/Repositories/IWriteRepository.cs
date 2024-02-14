using System;
using System.Threading.Tasks;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Domain.Repositories
{
    public interface IWriteBaseRepository<TEntity, TKey> : IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<int> SaveChangesAsync();
    }

    public interface IWriteRepository<TEntity, TKey> : IWriteBaseRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {

    }
}
