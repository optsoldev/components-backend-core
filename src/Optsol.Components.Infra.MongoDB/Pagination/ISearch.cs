using System;
using MongoDB.Driver;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.MongoDB.Pagination;

public interface ISearch<TEntity>
    where TEntity : IEntity
{
    Func<FilterDefinitionBuilder<TEntity>, FilterDefinition<TEntity>> GetSearcher();
}