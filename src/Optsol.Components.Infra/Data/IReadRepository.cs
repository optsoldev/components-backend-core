using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.Data
{
    public interface IReadRepository<TEntity, TKey> : IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);
        IAsyncEnumerable<TEntity> GetAllAsync();
        Task<SearchResult<TEntity>> GetAllAsync<TSearch>(IRequestSearch<TSearch> requestSearch) where TSearch : class;
    }
}
