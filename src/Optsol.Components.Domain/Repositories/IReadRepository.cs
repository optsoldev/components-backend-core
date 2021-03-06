using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Domain.Data
{
    public interface IReadBaseRepository<TEntity, TKey> : IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);

        Task<IEnumerable<TEntity>> GetAllAsync();
        
        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids);
    }

    public interface IReadRepository<TEntity, TKey> : IReadBaseRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes);

        Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes);

        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> Includes);
        
        Task<ISearchResult<TEntity>> GetAllAsync<TSearch>(ISearchRequest<TSearch> requestSearch) where TSearch : class;
    }
}