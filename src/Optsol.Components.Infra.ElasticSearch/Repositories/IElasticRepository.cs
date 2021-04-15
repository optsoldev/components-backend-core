using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.ElasticSearch.Context;

namespace Optsol.Components.Infra.ElasticSearch.Repositories
{
    public interface IElasticRepository<TEntity, TKey> :
        IReadBaseRepository<TEntity, TKey>,
        IWriteBaseRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        ElasticContext Context { get; }
    }
}
