using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IReadBaseRepository<TEntity, TKey> : IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<SearchResult<TEntity>> GetAllAsync<TSearch>(SearchRequest<TSearch> requestSearch) where TSearch : class;
    }

    public interface IReadRepository<TEntity, TKey> : IReadBaseRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes);

        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids);

        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes);

        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes);
    }
}