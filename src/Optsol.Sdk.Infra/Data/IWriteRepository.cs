using System;
using System.Threading.Tasks;
using Optsol.Sdk.Domain;

namespace Optsol.Sdk.Infra.Data
{
    public interface IWriteRepository<TEntity, TKey>
        where TEntity: class, IAggregateRoot<TKey>
    {
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task Delete(TEntity entity);
        Task Delete(TKey id);
        Task<int> SaveChanges();
    }
}
