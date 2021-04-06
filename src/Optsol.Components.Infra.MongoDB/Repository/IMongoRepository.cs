using MongoDB.Driver;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.MongoDB.Context;

namespace Optsol.Components.Infra.MongoDB.Repository
{
    public interface IMongoRepository<TEntity, TKey> :
        IReadBaseRepository<TEntity, TKey>,
        IWriteBaseRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        MongoContext Context { get; }

        IMongoCollection<TEntity> Set { get; }
    }
}
