using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Domain;

namespace Optsol.Components.Infra.Data
{
    public interface IReadRepository<TEntity, TKey> : IDisposable
        where TEntity: class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);
        IAsyncEnumerable<TEntity> GetAllAsync();
    }
}
