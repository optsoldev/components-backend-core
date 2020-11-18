using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IWriteRepository<TEntity, TKey> : IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity, Action<DbContext, TEntity> track);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<int> SaveChanges();
    }
}
