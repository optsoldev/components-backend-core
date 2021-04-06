using Optsol.Components.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Domain.Data
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
