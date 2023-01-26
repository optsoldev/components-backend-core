using System;
using MongoDB.Driver;
using Optsol.Components.Domain.Entities;

namespace Optsol.Components.Infra.MongoDB.Pagination;

public interface IOrderBy<TEntity> 
    where TEntity: IEntity
{
    Func<SortDefinitionBuilder<TEntity>, SortDefinition<TEntity>> GetOrderBy();
}