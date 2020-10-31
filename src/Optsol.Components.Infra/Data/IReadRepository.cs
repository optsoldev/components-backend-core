using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Components.Domain;

namespace Optsol.Components.Infra.Data
{
    //Todo: Mover para camada de Domain

    public interface IReadRepository<TEntity, TKey> : IDisposable
        where TEntity : class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);
        IAsyncEnumerable<TEntity> GetAllAsync();
    }
}
