using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.MongoDB.Repository
{
    public interface IMongoReadRepository<TEntity, TKey> where TEntity : class, IAggregateRoot<TKey>
    {
    }
}