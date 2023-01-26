using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Pagination;

namespace Optsol.Components.Domain.Repositories;

public interface IReadBaseRepository<TEntity, in TKey> : IDisposable
    where TEntity : class, IAggregateRoot<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id);

    Task<IEnumerable<TEntity>> GetAllAsync();
        
    Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids);
    Task<ISearchResult<TEntity>> GetAllAsync<TSearch>(ISearchRequest<TSearch> requestSearch) where TSearch : class;
}

public interface IReadRepository<TEntity, TKey> : IReadBaseRepository<TEntity, TKey>
    where TEntity : class, IAggregateRoot<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);

    Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<TKey> ids, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);

    Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);
}