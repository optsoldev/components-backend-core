using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optsol.Sdk.Domain;

namespace Optsol.Sdk.Infra.Data
{
    public interface IReadRepository<TEntity, TKey>
        where TEntity: class, IAggregateRoot<TKey>
    {
        Task<TEntity> GetById(TKey id);
        IAsyncEnumerable<TEntity> GetAllAsync();
    }
}
