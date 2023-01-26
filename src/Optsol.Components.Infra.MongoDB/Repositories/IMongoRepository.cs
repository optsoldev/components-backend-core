using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using Optsol.Components.Domain.Data;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Domain.Pagination;
using Optsol.Components.Domain.Repositories;
using Optsol.Components.Infra.MongoDB.Context;

namespace Optsol.Components.Infra.MongoDB.Repositories
{
    public interface IMongoRepository<TEntity, TKey> :
        IReadBaseRepository<TEntity, TKey>,
        IWriteBaseRepository<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
    {
        MongoContext Context { get; }

        IMongoCollection<TEntity> Set { get; }
        
        TEntity GetById(TKey id);

        IEnumerable<TEntity> GetAllByIds(params TKey[] ids);
        
        IEnumerable<TEntity> GetAll();
        
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filterExpression);
        
        ISearchResult<TEntity> GetAll<TSearch>(ISearchRequest<TSearch> searchRequest) where TSearch : class;
    }
}
