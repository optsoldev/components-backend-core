using System.Threading.Tasks;
using Optsol.Components.Domain;

namespace Optsol.Components.Infra.Data
{
    public interface IWriteRepository<TEntity, TKey>
        where TEntity: class, IAggregateRoot<TKey>
    {
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<int> SaveChanges();
    }
}
